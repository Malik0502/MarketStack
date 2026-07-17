using System.Text.Json.Serialization;

namespace MarketStack.Library.Contracts.Receipt.Dto.Lidl;

public class LidlReceiptPriceImportDto
{
    [JsonPropertyName("data-tax-type")]
    public required char TaxType { get; set; }

    [JsonPropertyName("data-tax-base-amount")]
    public required string TaxBaseAmount { get; set; }

    [JsonPropertyName("data-tax-amount")]
    public required string TaxAmount { get; set; }
}