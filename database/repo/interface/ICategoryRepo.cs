public interface ICategoryRepo : IBaseRepo<CategoryEntity>
{
    Task<CategoryEntity?> GetByNameAsync(string name);
}
