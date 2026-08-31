using MarketStack.Common.ResponseBase;

namespace MarketStack.Logic.Contracts.Service;

public interface IReceiptAnalysisService
{
    public Task<DataResponse<int>> GetTotalPurchases();

    public Task<DataResponse<decimal>> GetAveragePurchaseValue();

    public Task<DataResponse<decimal>> GetAverageItemsPerPurchase();

    public Task<DataResponse<decimal>> GetDiscountedItemShare();
}