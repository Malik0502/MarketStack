using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Contracts.Token;

namespace MarketStack.Library.Receipt
{
    public class LidlReceiptClient : IReceiptClient
    {
        private const string BaseApiUrl = "https://www.lidl.de";
        private const string AuthTokenApiUrl = $"{BaseApiUrl}/mla/api/v1/token";
        private const string AllReceiptApiUrl = $"{BaseApiUrl}/mre/api/v1/tickets?country";
        private const string ReceiptBaseUrl = $"{BaseApiUrl}/mre/api/v1/tickets";

        private static HttpClient _httpClient;
        private static HttpClientHandler _httpClientHandler;

        private static string _authToken = string.Empty;
        
        public LidlReceiptClient()
        {
            _httpClientHandler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };
            _httpClient = new HttpClient(_httpClientHandler);
        }
        
        public async Task<string?> GetAuthTokenAsync()
        {
            try
            {
                var json = await ExtractJsonFromApiCallAsync(AuthTokenApiUrl);

                if (string.IsNullOrEmpty(json))
                    return null;

                var token = DeserializeJson<LidlApiAuth>(json);

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
                var token = await GetAuthTokenAsync();
                if (token == null)
                    return null;

                var culture = CultureInfo.GetCultureInfo(languageCode);

                var apiUrl = $"{ReceiptBaseUrl}/{ticketId}?country={culture.TwoLetterISOLanguageName}&languageCode={languageCode}";

                var json = await ExtractJsonFromApiCallAsync(apiUrl);

                if (string.IsNullOrEmpty(json))
                    return null;

                var test = DeserializeJson<ReceiptDto>(json);
            }
            catch(CultureNotFoundException e)
            {
                Console.WriteLine($"Could not found a culture from the given language code: {e}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

            return null;
        }

        

        public async Task<ReceiptPageInfoDto?> GetReceiptsInfoAsync()
        {
            const int firstPage = 1;
            
            var token =  await GetAuthTokenAsync();

            if (string.IsNullOrEmpty(token))
                return null;
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var apiUrl = $"{AllReceiptApiUrl}=DE&page=";

            var jsonResponse = await ExtractJsonFromApiCallAsync(apiUrl + firstPage);

            if (string.IsNullOrEmpty(jsonResponse))
                return null;

            var receiptPageInfo = DeserializeJson<ReceiptPageInfoDto>(jsonResponse);

            if (receiptPageInfo == null)
                return null;
            
            for (int page = 2; page <= receiptPageInfo.TotalCount / receiptPageInfo.Size + 1; page++)
            {
                var json = await ExtractJsonFromApiCallAsync(apiUrl + page);

                if (string.IsNullOrEmpty(json))
                    continue;

                var receiptInfo = DeserializeJson<ReceiptPageInfoDto>(json);

                if (receiptInfo == null)
                    continue;
                
                receiptPageInfo.Items.AddRange(receiptInfo.Items);
            }
            
            return receiptPageInfo;
        }

        private static async Task<string?> ExtractJsonFromApiCallAsync(string apiUrl)
        {
            var response = await FetchAsync(apiUrl);

            if (response == null)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return json;
        }

        private static async Task<HttpResponseMessage?> FetchAsync(string apiUrl)
        {
            _httpClientHandler.CookieContainer.Add(new Uri(BaseApiUrl),
                new Cookie("authToken", _authToken));

            var response = await _httpClient.GetAsync(apiUrl);

            if (!response.IsSuccessStatusCode)
                return null;

            return response;
        }

        private static T? DeserializeJson<T>(string json) where T : class
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var result = JsonSerializer.Deserialize<T>(json, serializeOptions);

            return result;
        }
    }
}
