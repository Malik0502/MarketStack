using MarketStack.Library.Contracts.Miscellaneous;

namespace MarketStack.Library.Contracts.Receipt.Dto;

/// <summary>
/// Class representing an item on a receipt
/// </summary>
public class ReceiptItemDto
{
    /// <summary>
    /// Key from db
    /// </summary>
    public string? ItemId { get; set; }

    /// <summary>
    /// Internal item id from the store
    /// </summary>
    public string? InternalTicketId { get; set; }

    /// <summary>
    /// Name of article
    /// </summary>
    public string? ArticleName { get; set; }

    /// <summary>
    /// Price of Article
    /// </summary>
    public decimal ArticlePrice { get; set; }

    /// <summary>
    /// Quantity of Article
    /// </summary>
    public decimal Quantity { get; set; }

    /// <summary>
    /// Type of tax -> 19%, 7% et cetera
    /// </summary>
    public TaxType TaxType { get; set; }

    /// <summary>
    /// Id of used coupon
    /// </summary>
    public string? PromotionId { get; set; }
}