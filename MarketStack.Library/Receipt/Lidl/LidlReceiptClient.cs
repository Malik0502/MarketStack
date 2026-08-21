using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Contracts.Token;
using MarketStack.Library.Helper.Json;
using System.Globalization;
using System.Net;
using System.Text.Json;
using MarketStack.Common.ApiBase;
using MarketStack.Library.Contracts.Helper;

namespace MarketStack.Library.Receipt.Lidl
{
    public class LidlReceiptClient : IReceiptClient
    {
        private readonly IFetchClient _fetchClient;
        private const string BaseApiUrl = "https://www.lidl.de";
        private const string AuthTokenApiUrl = $"{BaseApiUrl}/mla/api/v1/token";
        private const string AllReceiptApiUrl = $"{BaseApiUrl}/mre/api/v1/tickets?country";
        private const string ReceiptBaseUrl = $"{BaseApiUrl}/mre/api/v1/tickets";

        private readonly HttpClient _httpClient;

        private static string _authToken =
            "eyJhbGciOiJSUzI1NiIsImtpZCI6IjdBQkE4MkIzRTQ4Qjg4MUI5QTg4MDU3N0ZBQUIwMTNENjIwOEYxMDNSUzI1NiIsInR5cCI6IkpXVCIsIng1dCI6ImVycUNzLVNMaUJ1YWlBVjMtcXNCUFdJSThRTSJ9.eyJuYmYiOjE3ODczMzA0MjIsImV4cCI6MTc4NzMzNDAyMiwiaXNzIjoiaHR0cHM6Ly9hY2NvdW50cy5saWRsLmNvbSIsImF1ZCI6WyJMaWRsLkF1dGhlbnRpY2F0aW9uIiwiaHR0cHM6Ly9hY2NvdW50cy5saWRsLmNvbS9yZXNvdXJjZXMiXSwiY2xpZW50X2lkIjoiR2VybWFueUVjb21tZXJjZUNsaWVudCIsInN1YiI6IjQxODIxOTgwMTE2NzY3MzE1IiwiYXV0aF90aW1lIjoxNzg3MzMwNDIyLCJpZHAiOiJsb2NhbCIsImxlZ2FsX3Rlcm1zIjoiREUiLCJzaWQiOiIxNzNGMkNFM0YzQjhGNUMxMUFCQTlEN0EyMkY2NzU4QSIsImlhdCI6MTc4NzMzMDQyMiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIkxpZGwuQXV0aGVudGljYXRpb24iLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIiwibWZhIl19.SmRFgxYZR672MdGLBC8uAiq7Z1HdRdt7oeUk8pL6dnePlPPBUsEclxZ-_dDZrfViddAaQHjISyyW30lRj9AcGgYB5Sm2S-y1sfjq1kZEK1ALhlotC2ABJ6B-ELQO6UF_IF_tjIZFH4jV68m1kuT4LLQMZH3eX5p0RbU3ctoXvDp_W3hN74eIwx_73jdnNMvCnTHJT1ysgCqNqUMkU65r5xM-vfH1qrXgX1mpinlHkhmOIRIzfF_dsJeDyzFmVjujlJQk31CqqCusst1L2U9Ylok__aweT5wkqIXyN4rwd9Heu_SC8qWF7LsilXH-yaLzHcwpfsfNK3jPOk8slE54hA";

        // TODO: write unittest to check for all possible error codes and if data response works like intended
        public LidlReceiptClient(IFetchClient fetchClient)
        {
            _fetchClient = fetchClient;
            var httpClientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = new CookieContainer(),
                AutomaticDecompression =
                    DecompressionMethods.GZip |
                    DecompressionMethods.Deflate |
                    DecompressionMethods.Brotli
            };

            _httpClient = new HttpClient(httpClientHandler);

            httpClientHandler.CookieContainer.Add(new Uri(BaseApiUrl),
                new Cookie("authToken", _authToken));
        }

        public async Task<DataResponse<string>> GetAuthTokenAsync()
        {
            try
            {
                var fetchedData = await _fetchClient.FetchJsonAsync(AuthTokenApiUrl, _httpClient);

                if (string.IsNullOrEmpty(fetchedData.Json))
                    return DataResponse<string>.CreateErrorResponse(
                            "Failed to retrieve authentication token.", 
                            "There was an error while fetching the authentication token.",
                            fetchedData.HttpResponseMessage.StatusCode.MapHttpStatusCodeToErrorCode());

                var token = JsonHelper.DeserializeJson<LidlApiAuth>(fetchedData.Json);

                if (token == null || string.IsNullOrEmpty(token.Token))
                    return DataResponse<string>.CreateErrorResponse(
                        "Failed to retrieve authentication token.",
                        "There was an error while deserializing the authentication token.",
                        ErrorCodes.ParseError);

                _authToken = token.Token;

                return DataResponse<string>.CreateSuccessResponse(
                    _authToken, 
                    "Success", 
                    "Authentication token retrieved successfully.");
            }
            catch (Exception e)
            {
                return DataResponse<string>.CreateErrorResponse("Exception occurred", e.Message, ErrorCodes.InternalError);
            }
        }

        public async Task<DataResponse<ReceiptDto>> GetReceiptAsync(string ticketId, string languageCode)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(languageCode);

                var apiUrl =
                    $"{ReceiptBaseUrl}/{ticketId}?country={culture.TwoLetterISOLanguageName}&languageCode={languageCode}";

                var fetchedData = await _fetchClient.FetchJsonAsync(apiUrl, _httpClient);

                if (string.IsNullOrEmpty(fetchedData.Json))
                    return DataResponse<ReceiptDto>.CreateErrorResponse(
                        "Failed to retrieve ticket.",
                        "There was an error while fetching the ticket information.",
                        fetchedData.HttpResponseMessage.StatusCode.MapHttpStatusCodeToErrorCode());

                using var document = JsonDocument.Parse(fetchedData.Json);

                var htmlPrintedReceipt = document.RootElement
                    .GetProperty("ticket")
                    .GetProperty("htmlPrintedReceipt")
                    .GetString()!;

                var date = document.RootElement
                    .GetProperty("ticket")
                    .GetProperty("date")
                    .GetString();

                var store = await GetReceiptsStoreLocationAsync(ticketId);

                var receiptItems = LidlReceiptParser.ParseToReceipt(htmlPrintedReceipt);

                if (receiptItems == null)
                    return DataResponse<ReceiptDto>.CreateErrorResponse(
                        "Failed to retrieve ticket.",
                        "There was an error while parsing the ticket information.",
                        ErrorCodes.ParseError);

                var receiptPriceInfoItems = LidlReceiptParser.ParseToReceiptPrice(htmlPrintedReceipt);

                if (receiptPriceInfoItems == null)
                    return DataResponse<ReceiptDto>.CreateErrorResponse(
                        "Failed to retrieve ticket.",
                        "There was an error while parsing the price information of a ticket.",
                        ErrorCodes.ParseError);

                var result = new ReceiptDto()
                {
                    TicketId = ticketId,
                    Currency = "€",
                    Date = date ?? string.Empty,
                    Store = store ?? string.Empty,
                    ReceiptItems = receiptItems,
                    ReceiptPriceInfos = receiptPriceInfoItems,
                    GrossPrice = receiptPriceInfoItems.Sum(x => x.TaxBaseAmount),
                };

                return DataResponse<ReceiptDto>.CreateSuccessResponse(
                    result, 
                    "Success", 
                    "Receipt data retrieved successfully.");
            }
            catch (CultureNotFoundException e)
            {
                return DataResponse<ReceiptDto>.CreateErrorResponse(
                    "Invalid language code.",
                    e.Message, ErrorCodes.Validation);
            }
            catch (Exception e)
            {
                return DataResponse<ReceiptDto>.CreateErrorResponse(
                    "Exception occurred",
                    e.Message, ErrorCodes.InternalError);
            }
        }

        public async Task<DataResponse<ReceiptPageInfoDto>> GetReceiptsInfoAsync()
        {
            try
            {
                const int firstPage = 1;

                var apiUrl = $"{AllReceiptApiUrl}=DE&page=";

                var fetchedData = await _fetchClient.FetchJsonAsync(apiUrl + firstPage, _httpClient);

                if (string.IsNullOrEmpty(fetchedData.Json))
                    return DataResponse<ReceiptPageInfoDto>.CreateErrorResponse(
                        "Failed to retrieve receipt information.",
                        "There was an error while fetching the receipt information.",
                        fetchedData.HttpResponseMessage.StatusCode.MapHttpStatusCodeToErrorCode());

                var receiptPageInfo = JsonHelper.DeserializeJson<ReceiptPageInfoDto>(fetchedData.Json);

                if (receiptPageInfo == null)
                    return DataResponse<ReceiptPageInfoDto>.CreateErrorResponse(
                        "Failed to retrieve receipt information.",
                        "There was an error while deserializing the receipt information.",
                        ErrorCodes.ParseError);

                for (int page = 2; page <= receiptPageInfo.TotalCount / receiptPageInfo.Size + 1; page++)
                {
                    fetchedData = await _fetchClient.FetchJsonAsync(apiUrl + page, _httpClient);

                    if (string.IsNullOrEmpty(fetchedData.Json))
                        continue;

                    var receiptInfo = JsonHelper.DeserializeJson<ReceiptPageInfoDto>(fetchedData.Json);

                    if (receiptInfo == null)
                        continue;

                    receiptPageInfo.Items.AddRange(receiptInfo.Items);
                }

                return DataResponse<ReceiptPageInfoDto>.CreateSuccessResponse(
                    receiptPageInfo,
                    "Success",
                    "Receipt information retrieved successfully."
                );
            }
            catch (Exception e)
            {
                return DataResponse<ReceiptPageInfoDto>.CreateErrorResponse(
                    "Exception occurred",
                    e.Message, ErrorCodes.InternalError);
            }
        }

        private async Task<string?> GetReceiptsStoreLocationAsync(string ticketId)
        {
            var receiptPageInfos = await GetReceiptsInfoAsync();

            if (receiptPageInfos.Data == null)
                return null;

            foreach (var receiptInfo in receiptPageInfos.Data.Items)
            {
                if (receiptInfo.Id != ticketId)
                    continue;

                return receiptInfo.Store;
            }

            return null;
        }
    }
}
