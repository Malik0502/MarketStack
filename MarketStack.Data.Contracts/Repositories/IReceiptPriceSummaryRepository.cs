using MarketStack.Data.Contracts.Entities;

namespace MarketStack.Data.Contracts.Repositories;

public interface IReceiptPriceSummaryRepository
{
    public Task<ICollection<ReceiptPriceSummary>> GetReceiptPriceSummariesAsync();
}