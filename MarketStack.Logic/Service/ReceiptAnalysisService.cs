using MarketStack.Common.ResponseBase;
using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;
using MarketStack.Logic.Contracts.Service;

namespace MarketStack.Logic.Service;

public class ReceiptAnalysisService : IReceiptAnalysisService
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IReceiptPriceSummaryRepository _receiptPriceSummaryRepository;
    private readonly IReceiptItemRepository _receiptItemRepository;

    public ReceiptAnalysisService(IReceiptRepository receiptRepository, IReceiptPriceSummaryRepository receiptPriceSummaryRepository, IReceiptItemRepository receiptItemRepository)
    {
        _receiptRepository = receiptRepository;
        _receiptPriceSummaryRepository = receiptPriceSummaryRepository;
        _receiptItemRepository = receiptItemRepository;
    }

    public async Task<DataResponse<int>> GetTotalPurchases()
    {
        int receiptCount = await _receiptRepository.GetTotalReceips();

        return DataResponse<int>.CreateSuccessResponse(receiptCount, "Success",
            "Successfully returned total purchases");
    }

    public async Task<DataResponse<decimal>> GetAveragePurchaseValue()
    {
        ICollection<ReceiptPriceSummary> priceSummaries =
            await _receiptPriceSummaryRepository.GetReceiptPriceSummariesAsync();
        
        int totalReceipts = await _receiptRepository.GetTotalReceips();

        decimal totalSum = priceSummaries.Sum(x => x.TaxBaseAmount);
        decimal result = totalSum / totalReceipts;

        return DataResponse<decimal>.CreateSuccessResponse(
            Math.Round(result, 2),
            "Success",
            "Successfully calculated the average purchase value");
    }

    public async Task<DataResponse<decimal>> GetAverageItemsPerPurchase()
    {
        ICollection<ReceiptItem> items = await _receiptItemRepository.GetReceiptItemsAsync();
        
        int totalReceipts = await _receiptRepository.GetTotalReceips();

        // weighed items like vegetables or fruits could falsify the result
        // there is no way of telling how many articles were weighed at the same time
        // we have to assume the consumer weighed everything seperatly
        // because of this we count each item with a fraction as 1 item even though it could be wrong
        decimal totalWeighedArticles = items.Count(x => x.Quantity % 1 != 0);

        decimal totalSumNormalArticles = items.Where(x => x.Quantity % 1 == 0).Sum(x => x.Quantity);


        decimal result = (totalSumNormalArticles + totalWeighedArticles) / totalReceipts;

        return DataResponse<decimal>.CreateSuccessResponse(
            Math.Round(result, 2),
            "Success", 
            "Successfully calculated the average items per purchase");
    }

    public async Task<DataResponse<decimal>> GetDiscountedItemShare()
    {
        ICollection<ReceiptItem> receiptItems = await _receiptItemRepository.GetReceiptItemsAsync();
        decimal itemsWithDiscount = 0;
        decimal itemCount = 0;

        foreach (var receiptItem in receiptItems)
        {
            // weighed items like vegetables or fruits will be counted as 1
            decimal itemQuantity = HasFraction(receiptItem.Quantity) ? 1 : receiptItem.Quantity;

            itemCount += itemQuantity;

            if (!string.IsNullOrEmpty(receiptItem.PromotionId))
                itemsWithDiscount += itemQuantity;
        }

        decimal result = itemCount == 0 ? 0 : (itemsWithDiscount / itemCount) * 100;

        return DataResponse<decimal>.CreateSuccessResponse(
            Math.Round(result, 2),
            "Success",
            "Successfully calculated the share of items with a discount");
    }

    private bool HasFraction(decimal quantity)
    {
        return quantity % 1 != 0;
    }
}