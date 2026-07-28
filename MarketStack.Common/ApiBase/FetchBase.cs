namespace MarketStack.Common.ApiBase;

public class FetchBase
{
    public HttpResponseMessage HttpResponseMessage { get; set; } = null!;

    public string? Json { get; set; }
}