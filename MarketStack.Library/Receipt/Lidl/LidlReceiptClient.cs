using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Contracts.Receipt.Dto.Lidl;
using MarketStack.Library.Contracts.Token;
using MarketStack.Library.Helper.Api;
using MarketStack.Library.Helper.Json;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using MarketStack.Library.Contracts.Miscellaneous;

namespace MarketStack.Library.Receipt.Lidl
{
    public class LidlReceiptClient : IReceiptClient
    {
        private const string BaseApiUrl = "https://www.lidl.de";
        private const string AuthTokenApiUrl = $"{BaseApiUrl}/mla/api/v1/token";
        private const string AllReceiptApiUrl = $"{BaseApiUrl}/mre/api/v1/tickets?country";
        private const string ReceiptBaseUrl = $"{BaseApiUrl}/mre/api/v1/tickets";

        private readonly HttpClient _httpClient;

        private static string _authToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjdBQkE4MkIzRTQ4Qjg4MUI5QTg4MDU3N0ZBQUIwMTNENjIwOEYxMDNSUzI1NiIsInR5cCI6IkpXVCIsIng1dCI6ImVycUNzLVNMaUJ1YWlBVjMtcXNCUFdJSThRTSJ9.eyJuYmYiOjE3ODI1NDI0NDIsImV4cCI6MTc4MjU0NjA0MiwiaXNzIjoiaHR0cHM6Ly9hY2NvdW50cy5saWRsLmNvbSIsImF1ZCI6WyJMaWRsLkF1dGhlbnRpY2F0aW9uIiwiaHR0cHM6Ly9hY2NvdW50cy5saWRsLmNvbS9yZXNvdXJjZXMiXSwiY2xpZW50X2lkIjoiR2VybWFueUVjb21tZXJjZUNsaWVudCIsInN1YiI6IjQxODIxOTgwMTE2NzY3MzE1IiwiYXV0aF90aW1lIjoxNzgyNTQyNDQyLCJpZHAiOiJsb2NhbCIsImxlZ2FsX3Rlcm1zIjoiREUiLCJzaWQiOiI4QjQwRTgxQUQ3NzVFNTVFNTBGMEYxNDA5NzdERENFNCIsImlhdCI6MTc4MjU0MjQ0Miwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsIkxpZGwuQXV0aGVudGljYXRpb24iLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIiwibWZhIl19.PKZGcfkOD1HrLsj7dy0FRnZlAOWC2Rwdj7O0mBOXDvVFHLR9WxUMXX1Zsxk6Xf9ZK1flDHgXKnrrwHpdrbSKKBfM4k768R06WMhwiYeMh_HaTeoNPcSUMgbfRw-IHDy_JX-BayP0v2l0RUQJfrKrYjTc8bpniJeQpqUVnYc6rTT2IwqNJo3PEbSMjjMIVFXdGumTaTyyBtDcN06iy0fuBk5dUVldfzsE4Y2gWBS_nNGyd-LSmU4XgbGYB5KA7UlZIbfJDFaD1DvtAPKQ7BAARcFBZ9sLVjncBr-gcBQLVxQNr7KZ20PRwmSwznwIvsTNWBhKBLMXmuHBF4aj5B6puw";

        private readonly Regex _htmlPattern = new("data-[a-zA-Z0-9_-]+=\"[^\"]*\"");

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

                var receiptItemsAsDictionary = ParseHtml(htmlPrintedReceipt);
                var receiptItems = ParseToReceipt(receiptItemsAsDictionary);

                if (receiptItems == null)
                    return null;

                var typeAGrossPrice = CalcPrice(receiptItems, TaxType.TypeA);
                var typeBGrossPrice = CalcPrice(receiptItems, TaxType.TypeB);
                var grossPrice = CalcPrice(receiptItems, TaxType.None);

                return new ReceiptDto()
                {
                    TicketId = ticketId,
                    Currency = "€",
                    ReceiptItems = receiptItems,
                    GrossPrice = grossPrice,
                    TypeAGrossPrice = typeAGrossPrice,
                    TypeATaxAmount = 0,
                    TypeBGrossPrice = typeBGrossPrice,
                    TypeBTaxAmount = 0
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

        private decimal CalcPrice(List<ReceiptItemDto> receiptItems, TaxType taxType)
        {
            decimal result = 0m;

            if (taxType == TaxType.None)
            {
                foreach (var receiptItem in receiptItems)
                {
                    result += receiptItem.ArticlePrice * receiptItem.Quantity;
                }

                return result;
            }

            // price calculation does not include coupon price reductions, yet
            foreach (var receiptItem in receiptItems)
            {
                if (receiptItem.TaxType != taxType)
                    continue;
                result += Math.Round(receiptItem.ArticlePrice * receiptItem.Quantity, 2);
            }

            return result;
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
                json = await ApiHelper.FetchJsonAsync(apiUrl + firstPage, _httpClient);

                if (string.IsNullOrEmpty(json))
                    continue;

                var receiptInfo = JsonHelper.DeserializeJson<ReceiptPageInfoDto>(json);

                if (receiptInfo == null)
                    continue;

                receiptPageInfo.Items.AddRange(receiptInfo.Items);
            }

            return receiptPageInfo;
        }

        private List<Dictionary<string, string>> ParseHtml(string html)
        {
            var matches = _htmlPattern.Matches(html).Select(x => x.Value).ToList();
            var dictionaries = new List<Dictionary<string, string>>();
            var dictionary = new Dictionary<string, string>();

            var isNewDictionary = true;

            for (int i = 0; i < matches.Count; i++)
            {
                var match = matches[i];
                var parts = match.Split("=", 2);
                if (parts.Length != 2)
                    continue;

                var key = parts[0].Trim('"');
                var value = parts[1].Trim('"');

                // skips all entries that are unrelated receiptItems such as the currency etc.
                if (!key.Contains("data-art-id", StringComparison.InvariantCultureIgnoreCase) &&
                    dictionaries.Count == 0)
                    continue;

                // every data-art-id marks a new object
                if (key.Contains("data-art-id", StringComparison.InvariantCultureIgnoreCase))
                    isNewDictionary = true;

                if (isNewDictionary)
                {
                    // prevents empty dictionaries in list
                    if (dictionary.Count != 0)
                        dictionaries.Add(dictionary);
                    dictionary = new Dictionary<string, string>();
                    dictionary.Add(key, value);
                    isNewDictionary = false;
                    continue;    
                }
                
                // prevents exceptions because of duplicate keys
                if (dictionary.ContainsKey(key))
                    continue;

                dictionary.Add(key, value);
            }
            return dictionaries;
        }
        
        private List<ReceiptItemDto>? ParseToReceipt(List<Dictionary<string, string>> receiptItemsAsDictionary)
        {
            var json = JsonHelper.SerializeJson(receiptItemsAsDictionary);

            var receiptImportItems = JsonHelper.DeserializeJson<List<LidlReceiptImportDto>>(json);

            if (receiptImportItems == null || receiptImportItems.Count == 0)
                return null;
            
            receiptImportItems = receiptImportItems.Where(x => !string.IsNullOrEmpty(x.ItemId) && !string.IsNullOrEmpty(x.ArticleName)).ToList();

            var receiptItems = new List<ReceiptItemDto>();

            foreach (var import in receiptImportItems)
            {
                var receipt = new ReceiptItemDto()
                {
                    ItemId = import.ItemId,
                    ArticleName = import.ArticleName,
                    ArticlePrice = Math.Round(decimal.Parse(import.ArticlePrice ?? "0", CultureInfo.CurrentCulture), 2),
                    PromotionId = import.PromotionId,
                    Quantity = decimal.Parse(string.IsNullOrEmpty(import.Quantity) ? "1" : import.Quantity, CultureInfo.CurrentCulture),
                    TaxType = TaxTypeConverter.CharToTaxType(import.TaxType), 
                };

                receiptItems.Add(receipt);
            }

            // removes duplicates and prioritizes items with a promotion ID
            return receiptItems
                .GroupBy(x => new { x.ItemId, x.Quantity })
                .Select(g =>
                    g.FirstOrDefault(x => x.PromotionId != null)
                    ?? g.First())
                .ToList();
        }
    }
}
