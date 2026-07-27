using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Contracts.Token;
using MarketStack.Library.Helper.Api;
using MarketStack.Library.Helper.Json;
using System.Globalization;
using System.Net;
using System.Text.Json;

namespace MarketStack.Library.Receipt.Lidl
{
    public class LidlReceiptClient : IReceiptClient
    {
        private const string BaseApiUrl = "https://www.lidl.de";
        private const string AuthTokenApiUrl = $"{BaseApiUrl}/mla/api/v1/token";
        private const string AllReceiptApiUrl = $"{BaseApiUrl}/mre/api/v1/tickets?country";
        private const string ReceiptBaseUrl = $"{BaseApiUrl}/mre/api/v1/tickets";

        private readonly HttpClient _httpClient;

        private static string _authToken =
            "";

        public LidlReceiptClient()
        {
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

        public async Task<string?> GetAuthTokenAsync()
        {
            try
            {
                var json = await ApiHelper.FetchJsonAsync(AuthTokenApiUrl, _httpClient);

                if (string.IsNullOrEmpty(json))
                    return null;

                var token = JsonHelper.DeserializeJson<LidlApiAuth>(json);

                if (token == null || string.IsNullOrEmpty(token.Token))
                    return null;

                _authToken = token.Token;

                return _authToken;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<ReceiptDto?> GetReceiptAsync(string ticketId, string languageCode)
        {
            try
            {
                var culture = CultureInfo.GetCultureInfo(languageCode);

                var apiUrl =
                    $"{ReceiptBaseUrl}/{ticketId}?country={culture.TwoLetterISOLanguageName}&languageCode={languageCode}";

                var json = await ApiHelper.FetchJsonAsync(apiUrl, _httpClient);

                if (string.IsNullOrEmpty(json))
                    return null;

                using var document = JsonDocument.Parse(json);

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
                    return null;

                var receiptPriceInfoItems = LidlReceiptParser.ParseToReceiptPrice(htmlPrintedReceipt);

                if (receiptPriceInfoItems == null)
                    return null;


                return new ReceiptDto()
                {
                    TicketId = ticketId,
                    Currency = "€",
                    Date = date ?? string.Empty,
                    Store = store ?? string.Empty,
                    ReceiptItems = receiptItems,
                    ReceiptPriceInfos = receiptPriceInfoItems,
                    GrossPrice = receiptPriceInfoItems.Sum(x => x.TaxBaseAmount),
                };
            }
            catch (CultureNotFoundException e)
            {
                Console.WriteLine($"Could not found a culture from the given language code: {e}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<ReceiptPageInfoDto?> GetReceiptsInfoAsync()
        {
            const int firstPage = 1;

            var apiUrl = $"{AllReceiptApiUrl}=DE&page=";

            var json = await ApiHelper.FetchJsonAsync(apiUrl + firstPage, _httpClient);

            if (string.IsNullOrEmpty(json))
                return null;

            var receiptPageInfo = JsonHelper.DeserializeJson<ReceiptPageInfoDto>(json);

            if (receiptPageInfo == null)
                return null;

            for (int page = 2; page <= receiptPageInfo.TotalCount / receiptPageInfo.Size + 1; page++)
            {
                json = await ApiHelper.FetchJsonAsync(apiUrl + page, _httpClient);

                if (string.IsNullOrEmpty(json))
                    continue;

                var receiptInfo = JsonHelper.DeserializeJson<ReceiptPageInfoDto>(json);

                if (receiptInfo == null)
                    continue;

                receiptPageInfo.Items.AddRange(receiptInfo.Items);
            }

            return receiptPageInfo;
        }

        private async Task<string?> GetReceiptsStoreLocationAsync(string ticketId)
        {
            var receiptPageInfos = await GetReceiptsInfoAsync();

            if (receiptPageInfos == null)
                return null;

            foreach (var receiptInfo in receiptPageInfos.Items)
            {
                if (receiptInfo.Id != ticketId)
                    continue;

                return receiptInfo.Store;
            }

            return null;
        }
    }
}
