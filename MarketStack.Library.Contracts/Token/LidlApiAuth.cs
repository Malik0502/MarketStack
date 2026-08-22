namespace MarketStack.Library.Contracts.Token;

/// <summary>
/// Class used to deserialize authentication token from json
/// </summary>
public class LidlApiAuth
{
    /// <summary>
    /// Lidl api authentication token
    /// </summary>
    public string Token { get; set; } = string.Empty;
}