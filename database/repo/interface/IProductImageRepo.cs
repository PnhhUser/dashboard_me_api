public interface IProductImageRepo : IBaseRepo<ProductImageEntity>
{
    Task AddRangeAsync(IEnumerable<ProductImageEntity> entities);

    Task<ProductImageEntity?> GetThumbnailAsync(int productId);

    Task<IEnumerable<ProductImageEntity>> GetByProductIdAsync(int productId);

    Task<int> GetMaxDisplayOrderAsync(int productId);

    Task<int> SetThumbnailByDisplayOrderAsync(int productId, int displayOrder);
}