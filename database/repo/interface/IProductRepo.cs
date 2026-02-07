public interface IProductRepo : IBaseRepo<ProductEntity>
{
    Task<ProductEntity?> GetByNameAsync(string name);

    Task<ProductEntity?> GetByCodeAsync(string code);
}