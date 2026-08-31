using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

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


    public Receipt GetReceiptAsync(int id)
    {
        return _context.Receipt.First(x => x.Id == id);
    }

    public async Task<List<Receipt>> GetReceiptsAsync()
    {
        return await _context.Receipt.ToListAsync();
    }

    public async Task<int> GetTotalReceips()
    {
        return await _context.Receipt.CountAsync();
    }

    public async Task<List<Receipt>> GetReceiptsIncludingPricesAsync()
    {
        return await _context.Receipt.Include(x => x.PriceSummaries).ToListAsync();
    }

    /// <summary>
    /// Adds a range of receipts as well as connected receipt-Items and products to the database
    /// </summary>
    /// <param name="receipts"></param>
    /// <returns></returns>
    public async Task AddReceiptRangeAsync(List<Receipt> receipts)
    {
        if (receipts.Count == 0)
            return;

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            // Get the purchase date of the latest receipt in the database.
            var lastPurchasedAt = await _context.Receipt
                .AsNoTracking()
                .MaxAsync(x => (DateTime?)x.PurchasedAt);

            // Lidl receipts are chronological, so every receipt after the
            // latest stored receipt must be new.
            var newReceipts = receipts
                .Where(x =>
                    !lastPurchasedAt.HasValue ||
                    x.PurchasedAt > lastPurchasedAt.Value)
                .GroupBy(x => new
                {
                    x.Chain,
                    x.ReceiptTicketId
                })
                .Select(x => x.First())
                .ToList();

            if (newReceipts.Count == 0)
            {
                await transaction.CommitAsync();
                return;
            }

            // Filter out possible receipt item duplicates inside receipt
            foreach (var receipt in newReceipts)
            {
                receipt.Items = receipt.Items
                    .GroupBy(x => new
                    {
                        ProductName = x.Product?.Name,
                        x.Quantity,
                        x.Price,
                        x.TaxType,
                        x.StoreInternItemId,
                        x.PromotionId
                    })
                    .Select(x => x.First())
                    .ToList();

                // Filter out duplicate price summaries inside receipt
                receipt.PriceSummaries = receipt.PriceSummaries
                    .GroupBy(x => new
                    {
                        x.TaxType,
                        x.TaxBaseAmount,
                        x.TaxAmount
                    })
                    .Select(x => x.First())
                    .ToList();
            }

            // All product names across new receipts
            var productNames = newReceipts
                .SelectMany(x => x.Items)
                .Select(x => x.Product?.Name)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            // Load existing products
            var existingProducts = await _context.Product
                .Where(x => productNames.Contains(x.Name))
                .ToListAsync();

            var productsByName = existingProducts
                .ToDictionary(x => x.Name);

            // Resolve products
            foreach (var receipt in newReceipts)
            {
                foreach (var item in receipt.Items)
                {
                    var productName = item.Product?.Name;

                    if (string.IsNullOrWhiteSpace(productName))
                    {
                        throw new InvalidOperationException(
                            "ReceiptItem has no product name");
                    }

                    if (productsByName.TryGetValue(
                            productName,
                            out var existingProduct))
                    {
                        item.Product = existingProduct;
                    }
                    else
                    {
                        var newProduct = new Product
                        {
                            Name = productName
                        };

                        _context.Product.Add(newProduct);

                        productsByName.Add(productName, newProduct);

                        item.Product = newProduct;
                    }
                }
            }

            _context.Receipt.AddRange(newReceipts);

            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

}