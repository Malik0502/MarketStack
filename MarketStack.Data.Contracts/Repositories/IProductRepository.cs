namespace MarketStack.Data.Contracts.Repositories;

public interface IProductRepository
{
    public Task AddTagAsync(string tagName);

    public Task AddTagToProductAsync(int tagId, int productId);

    public void RemoveTagFromProduct(int tagId, int productId);

    public void DeleteTag(int tagId);
}