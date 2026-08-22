using MarketStack.Data.Contracts.Entities;
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
            Chain = receiptDto.Chain,
            PriceSummaries = receiptDto.ReceiptPriceInfos.ToReceiptPriceSummaries()
        };
    }

    public static List<Receipt> ToReceipts(this List<ReceiptDto> receiptDtos)
    {
        var result = new List<Receipt>();

        foreach (var receiptDto in receiptDtos)
        {
            result.Add(receiptDto.ToReceipt());
        }

        return result;
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
            StoreInternItemId = receiptItemDto.ItemId,
            
        };
    }

    public static ReceiptPriceSummary ToReceiptPriceSummary(this ReceiptPriceInfo priceInfo)
    {
        return new ReceiptPriceSummary()
        {
            Id = 0,
            ReceiptId = 0,
            TaxType = (int)priceInfo.TaxType,
            TaxAmount = priceInfo.TaxAmount,
            TaxBaseAmount = priceInfo.TaxBaseAmount
        };
    }

    public static List<ReceiptPriceSummary> ToReceiptPriceSummaries(this List<ReceiptPriceInfo> receiptDtos)
    {
        var result = new List<ReceiptPriceSummary>();

        foreach (var receiptDto in receiptDtos)
        {
            result.Add(receiptDto.ToReceiptPriceSummary());
        }

        return result;
    }
}