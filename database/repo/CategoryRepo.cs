using Microsoft.EntityFrameworkCore;

public class CategoryRepo : BaseRepo<CategoryEntity>, ICategoryRepo
{
    public CategoryRepo(MeContext ctx) : base(ctx) { }


    public async Task<CategoryEntity?> GetByNameAsync(string name)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Name == name);
    }
}