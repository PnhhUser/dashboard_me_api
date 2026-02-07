public interface IProductService
{
    Task<IReadOnlyList<ProductModel>> GetAllAsync();

    Task<ProductModel> GetByIdAsync(int id);

    Task<ProductModel> AddAsync(CreateProductDTO dto);

    Task<ProductModel> EditAsync(EditProductDTO dto);

    Task<ProductModel> RemoveAsync(int id);
}

