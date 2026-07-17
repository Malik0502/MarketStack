using MarketStack.Library.Contracts.Miscellaneous;

namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptItemDto
{
    public string? ItemId { get; set; }

    public string? ArticleName { get; set; }

    public decimal ArticlePrice { get; set; }

    public decimal Quantity { get; set; }

    public TaxType TaxType { get; set; }

    public string? PromotionId { get; set; }
}