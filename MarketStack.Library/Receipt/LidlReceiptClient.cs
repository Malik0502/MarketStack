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
                _httpClientHandler.CookieContainer.Add(new Uri(BaseApiUrl),
                    new Cookie("authToken", _authToken));
            
                var tokenResponse = await _httpClient.GetAsync(AuthTokenApiUrl);
            
                var json = await tokenResponse.Content.ReadAsStringAsync();
            
                var serializeOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
            
                var token = JsonSerializer.Deserialize<LidlApiAuth>(json, serializeOptions);

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

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var culture = CultureInfo.GetCultureInfo(languageCode);

                var apiUrl = $"{ReceiptBaseUrl}/{ticketId}?country={culture.TwoLetterISOLanguageName}&languageCode={languageCode}";

                var response = await _httpClient.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
            }
            catch(CultureNotFoundException e)
            {
                Console.WriteLine("Could not found a culture from the given language code.");
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
            var pageNumber = 1;
            
            var token =  await GetAuthTokenAsync();

            if (token == null)
                return null;
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var apiUrl = $"{AllReceiptApiUrl}=DE&page=";
            
            var firstPageResponse = await _httpClient.GetAsync(apiUrl + pageNumber);
            
            if (!firstPageResponse.IsSuccessStatusCode)
            {
                return null;
            }
            
            var json = await firstPageResponse.Content.ReadAsStringAsync();
            
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var receiptPageInfo = JsonSerializer.Deserialize<ReceiptPageInfoDto>(json, serializeOptions);

            if (receiptPageInfo == null)
                return null;
            
            for (int i = 2; i <= receiptPageInfo.TotalCount / receiptPageInfo.Size + 1; i++)
            {
                var response = await _httpClient.GetAsync(apiUrl + i);
                var jsonString = await response.Content.ReadAsStringAsync();
                
                var receiptInfo =  JsonSerializer.Deserialize<ReceiptPageInfoDto>(jsonString, serializeOptions);

                if (receiptInfo == null)
                    continue;
                
                receiptPageInfo.Items.AddRange(receiptInfo.Items);
            }
            
            return receiptPageInfo;
        }
    }
}
