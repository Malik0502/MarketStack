using MarketStack.Common.ApiBase;
using MarketStack.Library.Contracts.Helper;

namespace MarketStack.Library.Helper.Api;

public class FetchClient : IFetchClient
{
    public async Task<FetchBase> FetchJsonAsync(string apiUrl, HttpClient httpClient)
    {
        return await ApiHelper.FetchJsonAsync(apiUrl, httpClient);
    }
}