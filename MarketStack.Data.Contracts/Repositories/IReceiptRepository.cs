using MarketStack.Data.Contracts.Entities;

namespace MarketStack.Data.Contracts.Repositories;

public interface IReceiptRepository
{
    public Task AddReceiptAsync(Receipt receipt);

    public Task AddReceiptRangeAsync(List<Receipt> receipts);

    public Receipt GetReceipt(int id);

    public Task<List<Receipt>> GetReceiptsAsync();
}