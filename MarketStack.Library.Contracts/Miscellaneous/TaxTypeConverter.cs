namespace MarketStack.Library.Contracts.Miscellaneous;

public static class TaxTypeConverter
{
    private static readonly Dictionary<TaxType, decimal> TaxDictionary = new()
    {
        { TaxType.TypeA, 0.07m },
        { TaxType.TypeB, 0.19m }
    };
    
    public static decimal GetTaxValue(TaxType taxType) => TaxDictionary[taxType];

    public static TaxType CharToTaxType(char taxType)
    {
        return taxType == 'A' ? TaxType.TypeA : TaxType.TypeB;
    }
}