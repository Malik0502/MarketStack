namespace MarketStack.Library.Contracts.Miscellaneous;

public static class TaxToValueConverter
{
    private static readonly Dictionary<char, decimal> TaxDictionary = new()
    {
        { 'A', 0.07m },
        { 'B', 0.19m }
    };
    
    public static decimal GetTaxValue(char taxType) => TaxDictionary[taxType];
}