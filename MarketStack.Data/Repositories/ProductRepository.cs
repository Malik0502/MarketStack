using MarketStack.Data.Contracts.Entities;
using MarketStack.Data.Contracts.Repositories;

namespace MarketStack.Data.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly MarketStackContext _context;

    public ProductRepository(MarketStackContext context)
    {
        _context = context;
    }

    public async Task AddTagAsync(string tagName)
    {
        var tag = new Tag() { Name = tagName };
        await _context.AddAsync(tag);
    }

    public async Task AddTagToProductAsync(int tagId, int productId)
    {
        var productTag = new ProductTag()
        {
            TagId = tagId,
            ProductId = productId
        };

        await _context.ProductTag.AddAsync(productTag);
    }

    public void RemoveTagFromProduct(int tagId, int productId)
    {
        var productTag = _context.ProductTag.Where(x => x.TagId == tagId && x.ProductId == productId).ToList();

        if (!productTag.Any())
            return;

        _context.ProductTag.RemoveRange(productTag);
        _context.SaveChanges();
    }

    public void DeleteTag(int tagId)
    {
        var entity = _context.Tag.FirstOrDefault(x => x.Id == tagId);

        if (entity == null)
            return;

        _context.Tag.Remove(entity);
        _context.SaveChanges();
    }
}