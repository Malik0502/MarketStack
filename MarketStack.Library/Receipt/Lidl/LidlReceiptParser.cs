using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Helper.TaxType;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MarketStack.Library.Receipt.Lidl;

public static class LidlReceiptParser
{
    private static readonly Regex HtmlPattern = new("data-[a-zA-Z0-9_-]+=\"[^\"]*\"");
    private static readonly Regex HtmlVatPattern = new("data-tax-[a-zA-Z0-9_-]+=\"[^\"]*\"");

    /// <summary>
    /// Parses the given receipt grocery items from an HTML string to specific objects
    /// </summary>
    /// <param name="htmlPrintedReceipt"></param>
    /// <returns>A list with specified objects containing grocery information from a receipt</returns>
    public static List<ReceiptItemDto>? ParseToReceipt(string? htmlPrintedReceipt)
    {
        try
        {
            var receiptItemDictionaries = ParseHtmlReceipt(htmlPrintedReceipt);

            if (receiptItemDictionaries == null || receiptItemDictionaries.Count == 0)
                return null;

            var receiptItems = receiptItemDictionaries.Select(x => new ReceiptItemDto()
            {
                ItemId = x.GetValueOrDefault("data-art-id", string.Empty),
                ArticleName = x!.GetValueOrDefault("data-art-description", null),
                ArticlePrice = Math.Round(decimal.Parse(x!.GetValueOrDefault("data-unit-price", null) ?? "0", CultureInfo.CurrentCulture), 2),
                PromotionId = x!.GetValueOrDefault("data-promotion-id", null),
                Quantity = Math.Round(decimal.Parse(string.IsNullOrEmpty(x.GetValueOrDefault("data-art-quantity", string.Empty)) ? "1" : x.GetValueOrDefault("data-art-quantity")!, CultureInfo.CurrentCulture), 3),
                TaxType = TaxTypeConverter.CharToTaxType(x.GetValueOrDefault("data-tax-type", string.Empty).FirstOrDefault())
            }).ToList();

            receiptItems = receiptItems
                .Where(x => !string.IsNullOrEmpty(x.ItemId) && !string.IsNullOrEmpty(x.ArticleName)).ToList();

            // removes duplicates and prioritizes items with a promotion ID
            return receiptItems
                .GroupBy(x => new { x.ItemId, x.Quantity })
                .Select(g =>
                    g.FirstOrDefault(x => !string.IsNullOrEmpty(x.PromotionId))
                    ?? g.First())
                .ToList();
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
            throw new FormatException(e.Message);
        }
    }

    /// <summary>
    /// Parses the grocery item information from an HTML to a dictionary list
    /// </summary>
    /// <param name="html"></param>
    /// <returns>A collection of dictionaries containing grocery data like the name, quantity and price etc.</returns>
    private static List<Dictionary<string, string>>? ParseHtmlReceipt(string? html)
    {
        if (string.IsNullOrEmpty(html))
            return null;

        var matches = HtmlPattern.Matches(html).Select(x => x.Value).ToList();
        var dictionaries = new List<Dictionary<string, string>>();
        var dictionary = new Dictionary<string, string>();

        var isNewDictionary = true;

        foreach (var match in matches)
        {
            var parts = match.Split("=", 2);
            if (parts.Length != 2)
                continue;

            var key = parts[0].Trim('"');
            var value = parts[1].Trim('"');

            // Each `data-art-id` marks the beginning of a new object.
            // Alternatively, any dictionary containing exactly six entries is treated as a new object.
            if (key.Contains("data-art-id", StringComparison.InvariantCultureIgnoreCase) || dictionary.Count == 6)
                isNewDictionary = true;

            if (isNewDictionary)
            {
                // prevents empty dictionaries in list
                if (dictionary.Count != 0)
                    dictionaries.Add(dictionary);
                dictionary = new Dictionary<string, string> { { key, value } };
                isNewDictionary = false;
                continue;
            }

            // prevents exceptions because of duplicate keys
            if (dictionary.ContainsKey(key))
                continue;

            dictionary.Add(key, value);
        }
        return dictionaries;
    }

    /// <summary>
    /// Parses the given price information from an HTML string to specific objects
    /// </summary>
    /// <param name="htmlPrintedReceipt"></param>
    /// <returns>A list with specified objects containing price information from different tax types</returns>
    public static List<ReceiptPriceInfo>? ParseToReceiptPrice(string? htmlPrintedReceipt)
    {
        try
        {
            var receiptPriceInfos = ParseHtmlPriceInfo(htmlPrintedReceipt);

            if (receiptPriceInfos == null)
                return null;

            var receiptPriceItems = receiptPriceInfos.Select(x => new ReceiptPriceInfo()
            {
                TaxType = TaxTypeConverter.CharToTaxType(x.GetValueOrDefault("data-tax-type", string.Empty).FirstOrDefault()),
                TaxBaseAmount = Math.Round(decimal.Parse(x!.GetValueOrDefault("data-tax-base-amount", null) ?? "0", CultureInfo.CurrentCulture), 2),
                TaxAmount = Math.Round(decimal.Parse(x!.GetValueOrDefault("data-tax-amount", null) ?? "0", CultureInfo.CurrentCulture), 2),
            }).ToList();

            if (receiptPriceItems.Count == 0)
                return null;

            return receiptPriceItems;
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
            throw new FormatException(e.Message);
        }
    }

    /// <summary>
    /// Parses the pricing and vat information from a lidl receipt HTML
    /// </summary>
    /// <param name="htmlPrintedReceipt"></param>
    /// <returns>
    /// A collection of N dictionaries, with N representing the total number of tax types applicable in the specified country.
    /// Each dictionary includes four key-value pairs describing the attributes of a single tax type.
    /// </returns>
    private static List<Dictionary<string, string>>? ParseHtmlPriceInfo(string? htmlPrintedReceipt)
    {
        if (string.IsNullOrEmpty(htmlPrintedReceipt))
            return null;

        // characters have to be on the top of the list
        var matches = HtmlVatPattern.Matches(htmlPrintedReceipt)
            .Select(x => x.Value.Trim('"'))
            .Distinct()
            .OrderBy(x => x.Contains("data-tax-type") ? 0 : 1)
            .ToList();

        var dictionaries = new List<Dictionary<string, string>>();
        var taxTypeAmount = CountTaxTypeAmount(matches);
        var editableTaxTypeAmount = taxTypeAmount;

        if (matches.Count != taxTypeAmount * 4)
            return null;

        for (int taxTypeIndex = 0; taxTypeIndex < taxTypeAmount; taxTypeIndex++)
        {
            var match = SplitMatch(matches[0]);
            var taxPercentage = SplitMatch(matches[editableTaxTypeAmount]);
            var taxBaseAmount = SplitMatch(matches[editableTaxTypeAmount + 1]);
            var taxAmount = SplitMatch(matches[editableTaxTypeAmount + 2]);

            var dictionary = new Dictionary<string, string>()
            {
                [match[0]] = match[1],
                [taxPercentage[0]] = taxPercentage[1],
                [taxBaseAmount[0]] = taxBaseAmount[1],
                [taxAmount[0]] = taxAmount[1],
            };

            dictionaries.Add(dictionary);

            matches.RemoveAt(editableTaxTypeAmount + 2);
            matches.RemoveAt(editableTaxTypeAmount + 1);
            matches.RemoveAt(editableTaxTypeAmount);
            matches.RemoveAt(0);
            editableTaxTypeAmount--;
        }
        return dictionaries;
    }

    /// <summary>
    /// Calculates the amount of different tax types.
    /// </summary>
    /// <param name="matches"></param>
    /// <returns>The amount of different tax types on the receipt</returns>
    private static int CountTaxTypeAmount(List<string> matches)
    {
        var result = 0;

        foreach (var match in matches)
        {
            var parts = match.Split("=", 2);

            if (parts.Length != 2)
                return result;

            var value = parts[1].Trim('"');

            if (value.Length == 1 && !int.TryParse(value, out _))
                result++;
        }
        return result;
    }

    /// <summary>
    /// Splits the found regex match in two parts
    /// </summary>
    /// <param name="match"></param>
    /// <returns>Returns a string array[2]</returns>
    private static string[] SplitMatch(string match)
    {
        var parts = match.Split("=", 2);

        if (parts.Length != 2)
            return [];

        parts[0] = parts[0].Trim('"');
        parts[1] = parts[1].Trim('"');

        return parts;
    }
}