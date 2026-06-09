using System.Globalization;
using System.Text.Json.Serialization;
using MarketStack.Library.Contracts.Miscellaneous;

namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptItemDto
{
    [JsonPropertyName("data-art-id")]
    public string? ItemId { get; set; }

    [JsonPropertyName("data-art-description")]
    public string? ArticleName { get; set; }

    [JsonInclude]
    [JsonPropertyName("data-unit-price")] 
    private string? _articlePriceString;

    public decimal ArticlePrice 
        => Math.Round(decimal.Parse(_articlePriceString ?? "0", CultureInfo.CurrentCulture), 2);

    [JsonInclude] 
    [JsonPropertyName("data-art-quantity")]
    private string? _quantityString;
    
    public decimal Quantity 
        => decimal.Parse(_quantityString ?? "1", CultureInfo.CurrentCulture);

    [JsonPropertyName("data-tax-type")]
    public char TaxType { get; set; }

    [JsonPropertyName("data-promotion-description")]
    public string? PromotionId { get; set; }
    
    public decimal TaxAmount 
        => Math.Round(ArticlePrice * TaxToValueConverter.GetTaxValue(TaxType), 2);
    
    public decimal PreTaxPrice 
        => Math.Round(ArticlePrice - TaxAmount, 2);
}