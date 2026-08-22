namespace MarketStack.Library.Contracts.Receipt.Dto;

/// <summary>
/// Class  representing a receipt as an object
/// </summary>
public class ReceiptDto
{
    /// <summary>
    /// Id of the receipt
    /// </summary>
    public required string TicketId { get; set; }

    /// <summary>
    /// Currency used
    /// </summary>
    public required string Currency { get; set; }

    /// <summary>
    /// Location visited
    /// </summary>
    public required string Store { get; set; }

    /// <summary>
    /// Storechain visited
    /// </summary>
    public required string Chain { get; set; }

    /// <summary>
    /// Date of purchase
    /// </summary>
    public required string Date { get; set; }
    
    /// <summary>
    /// Price to pay (after VAT)
    /// </summary>
    public required decimal GrossPrice { get; set; }

    /// <summary>
    /// Articles bought in the store
    /// </summary>
    public List<ReceiptItemDto> ReceiptItems { get; set; } = [];

    /// <summary>
    /// Information about VAT
    /// </summary>
    public List<ReceiptPriceInfo> ReceiptPriceInfos { get; set; } = [];
}