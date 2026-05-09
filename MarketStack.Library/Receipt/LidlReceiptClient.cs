using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using MarketStack.Library.Contracts.Receipt;
using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Library.Receipt
{
    public class LidlReceiptClient : IReceiptClient
    {
        private const string AUTH_TOKEN_API_URL = "https://www.lidl.de/mla/api/v1/token";
        private const string ALL_RECEIPT_API_URL = "https://www.lidl.de/mre/api/v1/tickets?country";

        private static HttpClient _httpClient = new();
        
        public async Task<string> GetAuthTokenAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ReceiptDto> GetReceiptAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ReceiptPageInfoDto?> GetReceiptsAsync()
        {
            var authToken = string.Empty;

            var pageNumber = 1;
            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            
            var apiUrl = "https://www.lidl.de/mre/api/v1/tickets?country=DE&page=";
            
            var firstPageResponse = await _httpClient.GetAsync(apiUrl +  pageNumber);

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            ;
            if (!firstPageResponse.IsSuccessStatusCode)
            {
                return null;
            }
            
            var json = await firstPageResponse.Content.ReadAsStringAsync();
            
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
