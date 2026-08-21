namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptDto
{
    public required string TicketId { get; set; }

    public required string Currency { get; set; }

    public required string Store { get; set; }

    public required string Chain { get; set; }

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