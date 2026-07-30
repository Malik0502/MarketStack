using MarketStack.Common.ApiBase;

namespace MarketStack.Library.Contracts.Helper;

public interface IFetchClient
{
    public Task<FetchBase> FetchJsonAsync(string apiUrl, HttpClient httpClient);
}