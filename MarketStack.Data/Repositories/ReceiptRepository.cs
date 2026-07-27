using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;

namespace MarketStack.Data.Repositories;

public class ReceiptRepository : IReceiptRepository
{
    private readonly MarketStackContext _context;

    public ReceiptRepository(MarketStackContext context)
    {
        _context = context;
    }

    public async Task AddReceiptAsync(Receipt receipt)
    {
        _context.Receipt.Add(receipt);
        await _context.SaveChangesAsync();
    }

    public Receipt GetReceipt(int id)
    {
        return _context.Receipt.First(x => x.Id == id);
    }
}