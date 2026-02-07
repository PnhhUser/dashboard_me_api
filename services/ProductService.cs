using Core.Errors;
using Core.Exceptions;

/// <summary>
/// Service for managing products
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    private readonly ICategoryRepo _categoryRepo;

    public ProductService(IProductRepo productRepo, ICategoryRepo categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of all active products</returns>
    public async Task<IReadOnlyList<ProductModel>> GetAllAsync()
    {
        var products = await _productRepo.GetAllAsync();

        return products.Select(ProductModel.ToModel).ToList();
    }

    /// <summary>
    /// Get a product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product model</returns>
    /// <exception cref="AppException">Thrown when product not found</exception>
    public async Task<ProductModel> GetByIdAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id);

        if (product == null)
        {
            throw new AppException(ErrorCode.NotFound, ErrorMessage.ProductNotFound);
        }

        return ProductModel.ToModel(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="dto">Product creation data</param>
    /// <returns>Created product model</returns>
    /// <exception cref="AppException">Thrown when validation fails or references don't exist</exception>
    public async Task<ProductModel> AddAsync(CreateProductDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.ProductIsRequired);
        }

        var exists = await _productRepo.GetByCodeAsync(dto.Code);

        if (exists != null)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.ProductCodeAlreadyExists
            );
        }

        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if (category == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.CategoryNotFound
            );
        }

        if (dto.Price <= 0)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.PriceInvalid
            );
        }

        var entity = new ProductEntity
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Code = dto.Code,
            Category = category,
            Active = dto.Active
        };

        await _productRepo.AddAsync(entity);
        await _productRepo.SaveAsync();

        return ProductModel.ToModel(entity);
    }

    /// <summary>
    /// Edit an existing product
    /// </summary>
    /// <param name="dto">Product edit data</param>
    /// <returns>Updated product model</returns>
    /// <exception cref="AppException">Thrown when validation fails or product not found</exception>
    public async Task<ProductModel> EditAsync(EditProductDTO dto)
    {
        var existed = await _productRepo.GetByIdAsync(dto.Id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.ProductNotFound
            );
        }

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.ProductIsRequired);
        }

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            existed.Name = dto.Name;
        }

        if (!string.IsNullOrWhiteSpace(dto.Description))
        {
            existed.Description = dto.Description;
        }

        var duplicateCode = await _productRepo.GetByCodeAsync(dto.Code);

        if (duplicateCode != null && duplicateCode.Id != existed.Id)
        {
            throw new AppException(
                    ErrorCode.ValidationError,
                    ErrorMessage.ProductCodeAlreadyExists
                );
        }
        else
        {
            existed.Code = dto.Code;
        }


        var category = await _categoryRepo.GetByIdAsync(dto.CategoryId);

        if (category == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.CategoryNotFound
            );
        }
        else
        {
            existed.Category = category;
        }


        if (dto.Price <= 0)
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.PriceInvalid
            );
        }


        existed.Price = dto.Price;
        existed.Active = dto.Active;
        existed.UpdatedAt = DateTime.UtcNow;


        await _productRepo.SaveAsync();

        return ProductModel.ToModel(existed);
    }

    /// <summary>
    /// Delete (soft delete) a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Deleted product model</returns>
    /// <exception cref="AppException">Thrown when product not found</exception>
    public async Task<ProductModel> RemoveAsync(int id)
    {
        var existed = await _productRepo.GetByIdAsync(id);

        if (existed == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                ErrorMessage.ProductNotFound
            );
        }

        existed.UpdatedAt = DateTime.UtcNow;
        existed.DeletedAt = DateTime.UtcNow;

        await _productRepo.SaveAsync();

        return ProductModel.ToModel(existed);
    }
}