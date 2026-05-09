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
        private const string AuthTokenApiUrl = "https://www.lidl.de/mla/api/v1/token";
        private const string AllReceiptApiUrl = "https://www.lidl.de/mre/api/v1/tickets?country";

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
                _httpClientHandler.CookieContainer.Add(new Uri("https://www.lidl.de"),
                    new Cookie("authToken", _authToken));
            
                var tokenResponse = await _httpClient.GetAsync("https://www.lidl.de/mla/api/v1/token");
            
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

        public async Task<ReceiptDto> GetReceiptAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ReceiptPageInfoDto?> GetReceiptsAsync()
        {
            var pageNumber = 1;
            
            var token =  await GetAuthTokenAsync();

            if (token == null)
                return null;
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var apiUrl = "https://www.lidl.de/mre/api/v1/tickets?country=DE&page=";
            
            var firstPageResponse = await _httpClient.GetAsync(apiUrl +  pageNumber);
            
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
