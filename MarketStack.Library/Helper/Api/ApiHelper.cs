using MarketStack.Common.ApiBase;
using MarketStack.Library.Helper.Json;

namespace MarketStack.Library.Helper.Api;

public static class ApiHelper
{
    public static async Task<FetchBase> FetchJsonAsync(string apiUrl, HttpClient httpClient)
    {
        var response = await FetchAsync(apiUrl, httpClient);

        var json = await JsonHelper.ExtractResponseAsJsonAsync(response);

        if (json == null)
            return new FetchBase
            {
                HttpResponseMessage = response!,
                Json = null
            };

        return new FetchBase
        {
            HttpResponseMessage = response!,
            Json = json
        };
    }
    private static async Task<HttpResponseMessage?> FetchAsync(string apiUrl, HttpClient httpClient)
    {
        var response = await httpClient.GetAsync(apiUrl);

        return response;
    }
}