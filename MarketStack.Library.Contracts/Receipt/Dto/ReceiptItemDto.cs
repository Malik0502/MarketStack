namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptItemDto
{
    public required string TicketId { get; set; }

    public required string ArticleName { get; set; }

    public required decimal ArticlePrice { get; set; }

    public required int Quantity { get; set; }

    public required string Currency { get; set; }

    public required char TaxType { get; set; }

    public required decimal TaxAmount { get; set; }

    public decimal PreTaxPrice { get; set; }

    public string? PromotionId { get; set; }

}