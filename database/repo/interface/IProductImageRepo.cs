public interface IProductImageRepo
{
    Task AddRangeAsync(IEnumerable<ProductImageEntity> entities);

    Task<ProductImageEntity?> GetThumbnailAsync(int productId);

    Task<IEnumerable<ProductImageEntity>> GetByProductIdAsync(int productId);
}