using MarketStack.Library.Receipt.Lidl;

namespace MarketStack.Library.Tests.Lidl.LidlReceiptParserTests
{
    public class InputOutputTests
    {
        #region Test data

        private readonly string _htmlReceipt = """
                                    <span id="purchase_list_line_2" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">Erdbeeren kg</span><span id="purchase_list_line_2" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">
                                    <span id="purchase_list_line_3" class="article" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">  </span><span id="purchase_list_line_3" class="article css_bold" data-art-id="0080795" data-art-quantity="0,472" data-unit-price="3,98" data-tax-type="A" data-art-description="Erdbeeren kg">0,472</span>
                                    <span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_4" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_4" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                                    <span id="purchase_list_line_5" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_5" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                                    <span id="purchase_list_line_6" class="article" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">  </span><span id="purchase_list_line_6" class="article css_bold" data-art-id="0080135" data-art-quantity="0,358" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">0,358</span>
                                    <span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">  </span><span id="purchase_list_line_7" class="discount css_bold" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1">   Lidl Plus Rabatt</span><span id="purchase_list_line_7" class="discount" data-promotion-id="100001001-DE-TEMPLATE-DESD000392189-1"></span>
                                    <span id="purchase_list_line_8" class="article css_bold" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg">Orangen kg</span><span id="purchase_list_line_8" class="article" data-art-id="0080135" data-art-quantity="0,35" data-unit-price="2,49" data-tax-type="A" data-art-description="Orangen kg"></span>
                                    """;

        private readonly string _htmlPriceInfoTwoTaxTypes = """
                                                            <span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38" class="css_bold">A</span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span>
                                                            <span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11" class="css_bold">B</span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span>
                                                            """;

        private readonly string _htmlPriceInfoFourTaxTypes = """
                                                            <span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38" class="css_bold">A</span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span><span id="vat_info_line_2" data-tax-type="A" data-tax-percentage="7" data-tax-base-amount="36,38" data-tax-amount="2,38"> </span>
                                                            <span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11" class="css_bold">B</span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span><span id="vat_info_line_3" data-tax-type="B" data-tax-percentage="19" data-tax-base-amount="0,71" data-tax-amount="0,11"> </span>
                                                            <span id="vat_info_line_4" data-tax-type="C" data-tax-percentage="10" data-tax-base-amount="3,12" data-tax-amount="0,55" class="css_bold">A</span><span id="vat_info_line_4" data-tax-type="C" data-tax-percentage="10" data-tax-base-amount="3,12" data-tax-amount="0,55"> </span><span id="vat_info_line_4" data-tax-type="C" data-tax-percentage="10" data-tax-base-amount="3,12" data-tax-amount="0,55"> </span>
                                                            <span id="vat_info_line_5" data-tax-type="D" data-tax-percentage="5" data-tax-base-amount="1,33" data-tax-amount="0,31" class="css_bold">B</span><span id="vat_info_line_5" data-tax-type="D" data-tax-percentage="5" data-tax-base-amount="1,33" data-tax-amount="0,31"> </span><span id="vat_info_line_5" data-tax-type="D" data-tax-percentage="5" data-tax-base-amount="1,33" data-tax-amount="0,31"> </span>
                                                            """;
        #endregion
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ParseToReceipt_ShouldReturnNull_WhenNullOrEmpty(string? input)
        {
            var result = LidlReceiptParser.ParseToReceipt(input);
            
            Assert.Null(result);
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ParseToReceiptPrice_ReturnNull_ForEmptyInput(string? input)
        {
            var result = LidlReceiptParser.ParseToReceiptPrice(input);
            
            Assert.Null(result);
        }
        
        [Fact]
        public void ParseToReceiptPrice_ShouldReturnTwoEntries_WhenGiven_HtmlWithTwoTaxTypes()
        {
            var result = LidlReceiptParser.ParseToReceiptPrice(_htmlPriceInfoTwoTaxTypes);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }
        
        [Fact]
        public void ParseToReceiptPrice_ShouldReturnTwoEntries_WhenGiven_HtmlWithFourTaxTypes()
        {
            var result = LidlReceiptParser.ParseToReceiptPrice(_htmlPriceInfoFourTaxTypes);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(4, result.Count);
        }
        
        [Fact]
        public void ParseToReceipt_ReturnThreeEntries_ForDictionaryInput()
        {
            var result = LidlReceiptParser.ParseToReceipt(_htmlReceipt);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count);
        }
    }
}
