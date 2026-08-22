namespace MarketStack.Library.Contracts.Receipt.Dto;

/// <summary>
/// Class representing the api result when searching through all receipts
/// </summary>
public class ReceiptPageInfoDto
{
    /// <summary>
    /// Amount items on a page
    /// </summary>
    public int Size { get; set; }

    // Amount of receipts in total
    public int TotalCount { get; set; }

    // Contains important receipt data for further processing
    public List<ReceiptInfoDto> Items { get; set; } = [];
}