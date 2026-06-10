using System.Text.Json.Serialization;

namespace MarketStack.Library.Contracts.Receipt.Lidl;

public class LidlReceiptImportDto
{
    [JsonPropertyName("data-art-id")]
    public string? ItemId { get; set; }

    [JsonPropertyName("data-art-description")]
    public string? ArticleName { get; set; }

    [JsonPropertyName("data-unit-price")]
    public string? ArticlePrice { get; set; }

    [JsonPropertyName("data-art-quantity")]
    public string? Quantity { get; set; }

    [JsonPropertyName("data-tax-type")]
    public char TaxType { get; set; }

    [JsonPropertyName("data-promotion-id")]
    public string? PromotionId { get; set; }
}