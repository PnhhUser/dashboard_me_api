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
}