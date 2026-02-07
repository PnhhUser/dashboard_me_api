public interface ICategoryService
{
    Task<IReadOnlyList<CategoryModel>> GetAllAsync();

    Task<CategoryModel> GetByIdAsync(int id);

    Task<CategoryModel> AddAsync(CreateCategoryDTO dto);

    Task<CategoryModel> EditAsync(EditCategoryDTO dto);

    Task<CategoryModel> RemoveAsync(int id);
}