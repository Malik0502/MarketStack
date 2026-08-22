namespace MarketStack.Library.Contracts.Receipt.Dto;

/// <summary>
/// Class representing receipt ticket id and store location for specific receipt calls
/// </summary>
public class ReceiptInfoDto
{
    /// <summary>
    /// Id of the receipt
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Location visited
    /// </summary>
    public string Store { get; set; } = string.Empty;
}