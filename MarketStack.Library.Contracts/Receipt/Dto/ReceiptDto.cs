namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptDto
{
    public required string TicketId { get; set; }

    public required string Currency { get; set; }
    
    /// <summary>
    /// Price to pay (after VAT)
    /// </summary>
    public required decimal GrossPrice { get; set; }

    /// <summary>
    /// Articles bought in the store
    /// </summary>
    public List<ReceiptItemDto> ReceiptItems { get; set; } = [];

    /// <summary>
    /// Informations about VAT
    /// </summary>
    public List<ReceiptPriceInfo> ReceiptPriceInfos { get; set; } = [];
}