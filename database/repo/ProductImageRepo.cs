using Microsoft.EntityFrameworkCore;

public class ProductImageRepo : BaseRepo<ProductImageEntity>, IProductImageRepo
{
    public ProductImageRepo(MeContext ctx) : base(ctx)
    { }

    public async Task AddRangeAsync(IEnumerable<ProductImageEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<ProductImageEntity?> GetThumbnailAsync(int productId)
    {
        return await _dbSet
                    .AsNoTracking()
                    .Where(x => x.ProductId == productId && x.IsPrimary)
                    .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ProductImageEntity>> GetByProductIdAsync(int productId)
    {
        return await _dbSet
                    .AsNoTracking()
                    .Where(x => x.ProductId == productId)
                    .ToListAsync();
    }

    public async Task<int> GetMaxDisplayOrderAsync(int productId)
    {
        var maxOrder = await _dbSet
            .Where(x => x.ProductId == productId)
            .MaxAsync(x => (int?)x.DisplayOrder);

        return maxOrder ?? -1;
    }

    public async Task<int> SetThumbnailByDisplayOrderAsync(int productId, int displayOrder)
    {
        // reset tất cả về false
        await _dbSet
            .Where(x => x.ProductId == productId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.IsPrimary, false)
                .SetProperty(p => p.UpdatedAt, DateTime.UtcNow));

        // set ảnh được chọn thành true
        var affectedRows = await _dbSet
            .Where(x => x.ProductId == productId
                     && x.DisplayOrder == displayOrder
                    )
            .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.IsPrimary, true)
                .SetProperty(p => p.UpdatedAt, DateTime.UtcNow));

        return affectedRows;
    }
}