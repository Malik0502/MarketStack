namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptDto
{
    public required string TicketId { get; set; }

    public required decimal TypeATaxAmount { get; set; }

    public required decimal TypeBTaxAmount { get; set; }

    public required decimal TypeAGrossPrice { get; set; }

    public required decimal TypeBGrossPrice { get; set; }

    public List<ReceiptItemDto> ReceiptItems { get; set; } = [];
}