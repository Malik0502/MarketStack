using System.Text.Json.Serialization;

namespace MarketStack.Library.Contracts.Receipt.Dto.Lidl;

/// <summary>
/// Class used to deserialize item of receipt from json
/// </summary>
public class LidlReceiptImportDto
{
    /// <summary>
    /// Internal item id
    /// </summary>
    [JsonPropertyName("data-art-id")]
    public string? ItemId { get; set; }

    /// <summary>
    /// Name of article
    /// </summary>
    [JsonPropertyName("data-art-description")]
    public string? ArticleName { get; set; }

    /// <summary>
    /// Price of article
    /// </summary>
    [JsonPropertyName("data-unit-price")]
    public string? ArticlePrice { get; set; }

    /// <summary>
    /// Quantity of article
    /// </summary>
    [JsonPropertyName("data-art-quantity")]
    public string? Quantity { get; set; }

    /// <summary>
    /// Type of tax -> 19% or 7% et cetera
    /// </summary>
    [JsonPropertyName("data-tax-type")]
    public char TaxType { get; set; }

    /// <summary>
    /// Id of used coupon
    /// </summary>
    [JsonPropertyName("data-promotion-id")]
    public string? PromotionId { get; set; }
}