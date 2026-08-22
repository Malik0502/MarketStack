using System.Text.Json.Serialization;

namespace MarketStack.Library.Contracts.Receipt.Dto.Lidl;

/// <summary>
/// Class used to deserialize price information from json
/// </summary>
public class LidlReceiptPriceImportDto
{
    /// <summary>
    /// Type of tax -> 19% or 7% et cetera
    /// </summary>
    [JsonPropertyName("data-tax-type")]
    public required char TaxType { get; set; }

    /// <summary>
    /// gross purchase price for the applicable VAT rate
    /// </summary>
    [JsonPropertyName("data-tax-base-amount")]
    public required string TaxBaseAmount { get; set; }

    /// <summary>
    /// tax amount included in the item's price
    /// </summary>
    [JsonPropertyName("data-tax-amount")]
    public required string TaxAmount { get; set; }
}