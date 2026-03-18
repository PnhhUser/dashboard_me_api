using Microsoft.EntityFrameworkCore;

public class ProductRepo : BaseRepo<ProductEntity>, IProductRepo
{
    public ProductRepo(MeContext ctx) : base(ctx) { }

    public async Task<ProductEntity?> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        var trimmed = name.Trim();
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == trimmed);
    }

    public async Task<ProductEntity?> GetByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return null;

        var normalized = code.Trim().ToUpperInvariant();
        return await _dbSet.FirstOrDefaultAsync(x => x.Code.ToUpper() == normalized);
    }

    public async Task<List<ProductEntity>> GetAllWithRelationAsync()
    {
        return await _dbSet
            .Include(x => x.Category)
            .Include(x => x.Images)
            .ToListAsync();
    }

    public async Task<ProductEntity?> GetByIdWithRelationAsync(int id)
    {
        return await _dbSet
            .Include(x => x.Category)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}