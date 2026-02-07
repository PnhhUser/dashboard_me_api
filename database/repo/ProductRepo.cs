using Microsoft.EntityFrameworkCore;

public class ProductRepo : BaseRepo<ProductEntity>, IProductRepo
{
    public ProductRepo(MeContext ctx) : base(ctx) { }

    public async Task<ProductEntity?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name && x.DeletedAt == null);
    }

    public async Task<ProductEntity?> GetByCodeAsync(string code)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Code == code && x.DeletedAt == null);
    }
}