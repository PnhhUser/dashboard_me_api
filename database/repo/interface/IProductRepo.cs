public interface IProductRepo : IBaseRepo<ProductEntity>
{
    Task<ProductEntity?> GetByNameAsync(string name);

    Task<ProductEntity?> GetByCodeAsync(string code);

    Task<List<ProductEntity>> GetAllWithRelationAsync();

    Task<ProductEntity?> GetByIdWithRelationAsync(int id);
}