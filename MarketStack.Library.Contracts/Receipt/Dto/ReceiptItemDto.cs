using MarketStack.Library.Contracts.Miscellaneous;

namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptItemDto
{
    public string? ItemId { get; set; }

    public string? ArticleName { get; set; }

    public decimal ArticlePrice { get; set; }

    public decimal Quantity { get; set; }

    public char TaxType { get; set; }

    public string? PromotionId { get; set; }

    public decimal PreTaxPrice 
        => Math.Round(ArticlePrice / (1 + TaxToValueConverter.GetTaxValue(TaxType)), 2);

    public decimal TaxAmount 
        => Math.Round(ArticlePrice - PreTaxPrice, 2);
}