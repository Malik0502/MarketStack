using MarketStack.Library.Contracts.Miscellaneous;
using MarketStack.Library.Contracts.Receipt.Dto;
using MarketStack.Library.Receipt.Lidl;

namespace MarketStack.Library.Tests.Lidl.LidlReceiptParserTests;

public class TechnicalTests
{
    [Fact]
    public void ParseToReceiptPrice_ShouldReturnTwoObjects_GivenTwoTaxTypes()
    {
        // Arrange
        var input = """
                    <span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38" class="css_bold">A</span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span>
                    <span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11" class="css_bold">B</span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span>
                    """;

        var expected = new List<ReceiptPriceInfo>()
        {
            new()
            {
                TaxType = TaxType.TypeA,
                TaxBaseAmount = 36.38m,
                TaxAmount = 2.38m
            },
            new()
            {
                TaxType = TaxType.TypeB,
                TaxBaseAmount = 0.71m,
                TaxAmount = 0.11m
            }
        };

        // Act
        var result = LidlReceiptParser.ParseToReceiptPrice(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Count, result.Count);
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public void ParseToReceiptPrice_ShouldThrowFormatException_GivenDataWithWrongQuantityFormat()
    {
        var input = """
                    <span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="ad" data-tax-amount="ac" class="css_bold">A</span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="ac" data-tax-amount="ac"> </span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="ac" data-tax-amount="ac"> </span>
                    <span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="ad" data-tax-amount="abd" class="css_bold">B</span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="ac" data-tax-amount="ac"> </span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="ac" data-tax-amount="ac"> </span>
                    """;

        Assert.Throws<FormatException>(() => LidlReceiptParser.ParseToReceiptPrice(input));
    }

    [Fact]
    public void ParseToReceiptPrice_ShouldReturnNull_GivenDataWithOddNumberMatches()
    {
        var input = """
                    <span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="1" data-tax-amount="1" class="css_bold">A</span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="1" data-tax-amount="1"> </span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="1" data-tax-amount="1"> </span>
                    <span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="1" data-tax-amount="1" class="css_bold">B</span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="1" data-tax-amount="1"> </span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="1" data-tax-amount="1"> </span>
                    """;

        var result = LidlReceiptParser.ParseToReceiptPrice(input);

        Assert.Null(result);
    }

    [Fact]
    public void ParseToReceipt_ShouldReturnFourObjects_GivenFourTaxTypes()
    {
        // Arrange
        var input = """
                    <span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38" class="css_bold">A</span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span>
                    <span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11" class="css_bold">B</span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span>
                    <span id="vat_info_line_4" data-tax-type="C" data-tax-percentage="10" data-tax-base-amount="3,12" data-tax-amount="0,55" class="css_bold">A</span><span id="vat_info_line_4" data-tax-type="C" data-tax-percentage="10" data-tax-base-amount="3,12" data-tax-amount="0,55"> </span><span id="vat_info_line_4" data-tax-type="C" data-tax-percentage="10" data-tax-base-amount="3,12" data-tax-amount="0,55"> </span>
                    <span id="vat_info_line_5" data-tax-type="D" data-tax-percentage="5" data-tax-base-amount="1,33" data-tax-amount="0,31" class="css_bold">B</span><span id="vat_info_line_5" data-tax-type="D" data-tax-percentage="5" data-tax-base-amount="1,33" data-tax-amount="0,31"> </span><span id="vat_info_line_5" data-tax-type="D" data-tax-percentage="5" data-tax-base-amount="1,33" data-tax-amount="0,31"> </span>
                    """;

        var expected = new List<ReceiptPriceInfo>()
        {
            new()
            {
                TaxType = TaxType.TypeA,
                TaxBaseAmount = 36.38m,
                TaxAmount = 2.38m
            },
            new()
            {
                TaxType = TaxType.TypeB,
                TaxBaseAmount = 0.71m,
                TaxAmount = 0.11m
            },
            new()
            {
                TaxType = TaxType.TypeC,
                TaxBaseAmount = 3.12m,
                TaxAmount = 0.55m
            },
            new()
            {
                TaxType = TaxType.TypeD,
                TaxBaseAmount = 1.33m,
                TaxAmount = 0.31m
            }
        };

        // Act
        var result = LidlReceiptParser.ParseToReceiptPrice(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Count, result.Count);
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public void ParseToReceipt_ShouldReturnThreeEntries_GivenDataWithDuplicates()
    {
        var input = ReturnParseToReceiptInput();

        var expected = new List<ReceiptItemDto>()
        {
            new()
            {
                ItemId = "0080795",
                ArticleName = "Erdbeeren kg",
                ArticlePrice = 3.98m,
                PromotionId = "100001001-DE-TEMPLATE-DESD000392189-1",
                Quantity = 0.472m,
                TaxType = TaxType.TypeA
            },
            new()
            {
                ItemId = "0080135",
                ArticleName = "Orangen kg",
                ArticlePrice = 2.49m,
                PromotionId = "100001001-DE-TEMPLATE-DESD000392189-1",
                Quantity = 0.358m,
                TaxType = TaxType.TypeA
            },
            new()
            {
                ItemId = "0080135",
                ArticleName = "Orangen kg",
                ArticlePrice = 2.49m,
                PromotionId = null,
                Quantity = 0.35m,
                TaxType = TaxType.TypeA
            }
        };

        var result = LidlReceiptParser.ParseToReceipt(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expected.Count, result.Count);
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public void ParseToReceipt_ShouldntReturnTheObjectWithoutArticleName_GivenDataWithoutArticleName()
    {
        var withoutArticleName = "Erdbeeren kg";
        var input = ReturnParseToReceiptInput(withoutArticleName);

        var expected = new List<ReceiptItemDto>()
        {
            new()
            {
                ItemId = "0080135",
                ArticleName = "Orangen kg",
                ArticlePrice = 2.49m,
                PromotionId = "100001001-DE-TEMPLATE-DESD000392189-1",
                Quantity = 0.358m,
                TaxType = TaxType.TypeA
            },
            new()
            {
                ItemId = "0080135",
                ArticleName = "Orangen kg",
                ArticlePrice = 2.49m,
                PromotionId = null,
                Quantity = 0.35m,
                TaxType = TaxType.TypeA
            }
        };

        var result = LidlReceiptParser.ParseToReceipt(input);

        Assert.NotNull(result);
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public void ParseToReceipt_ShouldntReturnTheObjectWithoutItemId_GivenDataWithoutItemId()
    {
        var withoutItemId = "data-art-id=\"0080135\"";
        var input = ReturnParseToReceiptInput(withoutItemId);

        var expected = new List<ReceiptItemDto>()
        {
            new()
            {
                ItemId = "0080795",
                ArticleName = "Erdbeeren kg",
                ArticlePrice = 3.98m,
                PromotionId = "100001001-DE-TEMPLATE-DESD000392189-1",
                Quantity = 0.472m,
                TaxType = TaxType.TypeA
            },
        };

        var result = LidlReceiptParser.ParseToReceipt(input);

        Assert.NotNull(result);
        Assert.Equivalent(expected, result);
    }

    [Fact]
    public void ParseToReceipt_ShouldThrowFormatException_GivenDataWithWrongQuantityFormat()
    {
        var substringToChange = "data-art-quantity=\"0,472\"";
        var substringToChangeInto = "data-art-quantity=\"abc\"";
        var input = ReturnParseToReceiptInput(substringToChange, substringToChangeInto);

        Assert.Throws<FormatException>(() => LidlReceiptParser.ParseToReceipt(input));
    }

    [Fact]
    public void ParseToReceipt_ShouldReturnNull_GivenDataWithZeroMatches()
    {
        var result = LidlReceiptParser.ParseToReceiptPrice(string.Empty);

        Assert.Null(result);
    }

    private string ReturnParseToReceiptInput()
    {
        var input = """
                        <span id="purchase_list_line_2" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">Erdbeeren kg</span><span id="purchase_list_line_2" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">
                        <span id="purchase_list_line_3" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">  </span><span id="purchase_list_line_3" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">0,472</span>
                        <span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_4" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                        <span id="purchase_list_line_5" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_5" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                        <span id="purchase_list_line_6" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">  </span><span id="purchase_list_line_6" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">0,358</span>
                        <span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_7" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                        <span id="purchase_list_line_8" class="article css_bold" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_8" class="article" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                        """;

        return input;
    }

    private string ReturnParseToReceiptInput(string substringToRemove)
    {
        var html = """
                        <span id="purchase_list_line_2" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">Erdbeeren kg</span><span id="purchase_list_line_2" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">
                        <span id="purchase_list_line_3" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">  </span><span id="purchase_list_line_3" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">0,472</span>
                        <span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_4" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                        <span id="purchase_list_line_5" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_5" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                        <span id="purchase_list_line_6" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">  </span><span id="purchase_list_line_6" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">0,358</span>
                        <span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_7" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                        <span id="purchase_list_line_8" class="article css_bold" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_8" class="article" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                        """;

        var input = html.Replace(substringToRemove, string.Empty);

        return input;
    }

    private string ReturnParseToReceiptInput(string substringToChange, string substringToChangeInto)
    {
        var html = """
                        <span id="purchase_list_line_2" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">Erdbeeren kg</span><span id="purchase_list_line_2" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">
                        <span id="purchase_list_line_3" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">  </span><span id="purchase_list_line_3" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">0,472</span>
                        <span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_4" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                        <span id="purchase_list_line_5" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_5" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                        <span id="purchase_list_line_6" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">  </span><span id="purchase_list_line_6" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">0,358</span>
                        <span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_7" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                        <span id="purchase_list_line_8" class="article css_bold" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_8" class="article" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                        """;

        var input = html.Replace(substringToChange, substringToChangeInto);

        return input;
    }
}