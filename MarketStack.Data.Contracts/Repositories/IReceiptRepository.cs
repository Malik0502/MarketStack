using MarketStack.Data.Contracts.Entities;

namespace MarketStack.Data.Contracts.Repositories;

public interface IReceiptRepository
{
    public Task AddReceiptAsync(Receipt receipt);
    public Receipt GetReceipt(int id);
}