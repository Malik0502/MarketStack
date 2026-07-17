using MarketStack.Library.Contracts.Miscellaneous;

namespace MarketStack.Library.Contracts.Receipt.Dto;

public class ReceiptPriceInfo
{
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