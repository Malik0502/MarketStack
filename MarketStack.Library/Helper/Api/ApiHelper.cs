using MarketStack.Library.Helper.Json;

namespace MarketStack.Library.Helper.Api;

public static class ApiHelper
{
    public static async Task<string?> FetchJsonAsync(string apiUrl, HttpClient httpClient)
    {
        var response = await FetchAsync(apiUrl, httpClient);

        var json = await JsonHelper.ExtractResponseAsJsonAsync(response);

        return json;
    }
    private static async Task<HttpResponseMessage?> FetchAsync(string apiUrl, HttpClient httpClient)
    {
        var response = await httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
            return null;

        return response;
    }
}