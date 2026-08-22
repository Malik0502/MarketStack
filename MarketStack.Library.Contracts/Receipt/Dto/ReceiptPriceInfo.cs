using MarketStack.Library.Contracts.Miscellaneous;

namespace MarketStack.Library.Contracts.Receipt.Dto;

/// <summary>
/// Class representing receipt price information
/// </summary>
public class ReceiptPriceInfo
{
    /// <summary>
    /// Type of tax -> 19%, 7% et cetera
    /// </summary>
    public required TaxType TaxType { get; set; }

    /// <summary>
    /// Price with Taxes
    /// </summary>
    public required decimal TaxBaseAmount { get; set; }

    /// <summary>
    /// Amount of VAT contained within the gross price
    /// </summary>
    public required decimal TaxAmount { get; set; }
}