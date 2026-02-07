using Core.Errors;
using Core.Exceptions;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _categoryRepo;

    public CategoryService(ICategoryRepo categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    // ---------- READ ----------
    public async Task<IReadOnlyList<CategoryModel>> GetAllAsync()
    {
        var categories = await _categoryRepo.GetAllAsync();

        return categories.Select(CategoryModel.ToModel).ToList();
    }

    public async Task<CategoryModel> GetByIdAsync(int id)
    {
        var category = await _categoryRepo.GetByIdAsync(id);

        if (category == null)
        {
            throw new AppException(ErrorCode.NotFound, ErrorMessage.CategoryNotFound);
        }

        return CategoryModel.ToModel(category);
    }

    // ---------- REMOVE ----------
    public async Task<CategoryModel> RemoveAsync(int id)
    {
        var existed = await _categoryRepo.GetByIdAsync(id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.CategoryNotFound
            );
        }

        existed.UpdatedAt = DateTime.UtcNow;
        existed.DeletedAt = DateTime.UtcNow;

        await _categoryRepo.SaveAsync();

        return CategoryModel.ToModel(existed);
    }

    // ---------- CREATE ----------
    public async Task<CategoryModel> AddAsync(CreateCategoryDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.CategoryIsRequired);
        }

        var existed = await _categoryRepo.GetByNameAsync(dto.Name);

        if (existed != null)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.CategoryAlreadyExists
            );
        }

        var entity = new CategoryEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow
        };

        await _categoryRepo.AddAsync(entity);
        await _categoryRepo.SaveAsync();

        return CategoryModel.ToModel(entity);
    }

    // ---------- EDIT ----------
    public async Task<CategoryModel> EditAsync(EditCategoryDTO dto)
    {
        var existed = await _categoryRepo.GetByIdAsync(dto.Id)
            ?? throw new AppException(ErrorCode.NotFound, ErrorMessage.CategoryNotFound);

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            var duplicated = await _categoryRepo.GetByNameAsync(dto.Name);
            if (duplicated != null && duplicated.Id != existed.Id)
            {
                throw new AppException(
                    ErrorCode.ValidationError,
                    ErrorMessage.CategoryAlreadyExists
                );
            }

            existed.Name = dto.Name;
        }

        if (!string.IsNullOrWhiteSpace(dto.Description))
        {
            existed.Description = dto.Description;
        }

        existed.UpdatedAt = DateTime.UtcNow;

        await _categoryRepo.SaveAsync();

        return CategoryModel.ToModel(existed);
    }
}