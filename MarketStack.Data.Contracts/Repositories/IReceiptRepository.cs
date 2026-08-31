using MarketStack.Data.Contracts.Entities;

namespace MarketStack.Data.Contracts.Repositories;

public interface IReceiptRepository
{
    public Task AddReceiptAsync(Receipt receipt);

    public Task AddReceiptRangeAsync(List<Receipt> receipts);

    public Receipt GetReceiptAsync(int id);

    public Task<List<Receipt>> GetReceiptsAsync();

    public Task<int> GetTotalReceips();

    public Task<List<Receipt>> GetReceiptsIncludingPricesAsync();
}