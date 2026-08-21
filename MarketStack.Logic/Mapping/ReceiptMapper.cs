using MarketStack.Data.Contracts.Entities;
using MarketStack.Library.Contracts.Miscellaneous;
using MarketStack.Library.Contracts.Receipt.Dto;

namespace MarketStack.Logic.Mapping;

public static class ReceiptMapper
{
    public static Receipt ToReceipt(this ReceiptDto receiptDto)
    {
        return new Receipt()
        {
            Id = 0,
            ReceiptTicketId = receiptDto.TicketId,
            Store = receiptDto.Store,
            PurchasedAt = DateTime.SpecifyKind(DateTime.Parse(receiptDto.Date), DateTimeKind.Local).ToUniversalTime(),
            Items = receiptDto.ReceiptItems.Select(x => x.ToReceiptItem()).ToList(),
        };
    }

    public static ReceiptItem ToReceiptItem(this ReceiptItemDto receiptItemDto)
    {
        return new ReceiptItem()
        {
            Id = 0,
            ProductId = 0,
            ReceiptId = 0,
            Price = receiptItemDto.ArticlePrice,
            Product = new Product() { Id = 0, Name = receiptItemDto.ArticleName! },
            Quantity = receiptItemDto.Quantity,
            TaxType = (int)receiptItemDto.TaxType,
            PromotionId = receiptItemDto.PromotionId,
            StoreInternItemId = receiptItemDto.ItemId
        };
    }

    public static ReceiptItemDto ToReceiptItemDto(this ReceiptItem receiptItem)
    {
        return new ReceiptItemDto()
        {
            ArticleName = receiptItem.Product!.Name,
            ArticlePrice = receiptItem.Price,
            ItemId = receiptItem.StoreInternItemId,
            Quantity = receiptItem.Quantity,
            TaxType = (TaxType)receiptItem.TaxType,
            PromotionId = receiptItem.PromotionId
        };
    }
}