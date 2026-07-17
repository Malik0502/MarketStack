namespace MarketStack.Library.Helper.TaxType;

public static class TaxTypeConverter
{
    public static Contracts.Miscellaneous.TaxType CharToTaxType(char taxType)
    {
        return taxType switch
        {
            'A' => Contracts.Miscellaneous.TaxType.TypeA,
            'B' => Contracts.Miscellaneous.TaxType.TypeB,
            'C' => Contracts.Miscellaneous.TaxType.TypeC,
            'D' => Contracts.Miscellaneous.TaxType.TypeD,
            _ => Contracts.Miscellaneous.TaxType.None
        };
    }
}