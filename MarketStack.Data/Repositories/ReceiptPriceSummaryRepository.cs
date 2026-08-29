using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketStack.Data.Repositories;

public class ReceiptPriceSummaryRepository : IReceiptPriceSummaryRepository
{
    private readonly MarketStackContext _context;

    public ReceiptPriceSummaryRepository(MarketStackContext context)
    {
        _context = context;
    }
    public async Task<ICollection<ReceiptPriceSummary>> GetReceiptPriceSummariesAsync()
    {
        return await _context.ReceiptPriceSummary.ToListAsync();
    }

    public async Task<ICollection<ReceiptPriceSummary>> GetReceiptPriceSummaryFromReceiptAsync(int id)
    {
        return await _context.ReceiptPriceSummary.Where(x => x.ReceiptId == id).ToListAsync();
    }
}