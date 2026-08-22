using MarketStack.Data.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Data.Contracts.Repositories;

public interface IReceiptItemRepository
{
    public Task<ICollection<ReceiptItem>> GetReceiptItemsAsync();
}