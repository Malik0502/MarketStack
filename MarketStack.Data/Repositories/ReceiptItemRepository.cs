using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Data.Repositories;

public class ReceiptItemRepository : IReceiptItemRepository
{
    private readonly MarketStackContext _context;

    public ReceiptItemRepository(MarketStackContext context)
    {
        _context = context;
    }

    public async Task<ICollection<ReceiptItem>> GetReceiptItemsAsync()
    {
        return await _context.ReceiptItem.ToListAsync();
    }
}