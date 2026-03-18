using Core.Errors;
using Core.Exceptions;

/// <summary>
/// Service for managing products
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    private readonly ICategoryRepo _categoryRepo;

    public ProductService(
        IProductRepo productRepo,
        ICategoryRepo categoryRepo
        )
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
        var products = await _productRepo.GetAllWithRelationAsync();

        return products.Select(MapToModel).ToList();
    }


    private ProductModel MapToModel(ProductEntity product)
    {
        var model = ProductModel.ToModel(
            product,
            product.Category != null ? CategoryModel.ToModel(product.Category) : null
        );

        model.Images = product.Images?
            .OrderBy(x => x.DisplayOrder)
            .Select(ProductImageModel.ToModel)
            .ToList() ?? new List<ProductImageModel>();

        return model;
    }

    /// <summary>
    /// Get a product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product model</returns>
    /// <exception cref="AppException">Thrown when product not found</exception>
    public async Task<ProductModel> GetByIdAsync(int id)
    {
        var product = await _productRepo.GetByIdWithRelationAsync(id);

        if (product == null)
            throw new AppException(ErrorCode.NotFound, ErrorMessage.ProductNotFound);

        if (product.Category == null)
            throw new AppException(ErrorCode.NotFound, ErrorMessage.CategoryNotFound);

        var model = ProductModel.ToModel(
            product,
            CategoryModel.ToModel(product.Category)
        );

        return MapToModel(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="dto">Product creation data</param>
    /// <returns>Created product model</returns>
    /// <exception cref="AppException">Thrown when validation fails or references don't exist</exception>
    public async Task<ProductModel> AddAsync(CreateProductDTO dto)
    {
        dto.Name = dto.Name?.Trim() ?? string.Empty;
        dto.Description = dto.Description?.Trim() ?? string.Empty;
        dto.Code = dto.Code?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new AppException(
                ErrorCode.ValidationError,
                ErrorMessage.ProductIsRequired);
        }

        // normalize code to upper so that uniqueness is case‑insensitive
        var normalizedCode = NormalizeCode(dto.Code);
        var exists = await _productRepo.GetByCodeAsync(normalizedCode);

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
            Code = normalizedCode,
            Category = category,
            Active = dto.Active
        };

        await _productRepo.AddAsync(entity);
        await _productRepo.SaveAsync();


        var categoryModel = CategoryModel.ToModel(category);

        return ProductModel.ToModel(entity, categoryModel);
    }




    /// <summary>
    /// Edit an existing product
    /// </summary>
    /// <param name="dto">Product edit data</param>
    /// <returns>Updated product model</returns>
    /// <exception cref="AppException">Thrown when validation fails or product not found</exception>
    public async Task<ProductModel> EditAsync(EditProductDTO dto)
    {
        dto.Name = dto.Name?.Trim() ?? string.Empty;
        dto.Description = dto.Description?.Trim() ?? string.Empty;
        dto.Code = dto.Code?.Trim() ?? string.Empty;

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

        existed.Name = dto.Name;
        existed.Description = dto.Description;

        var duplicateCode = await _productRepo.GetByCodeAsync(NormalizeCode(dto.Code));

        if (duplicateCode != null && duplicateCode.Id != existed.Id)
        {
            throw new AppException(
                    ErrorCode.ValidationError,
                    ErrorMessage.ProductCodeAlreadyExists
                );
        }
        else
        {
            existed.Code = NormalizeCode(dto.Code);
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

        var categoryModel = CategoryModel.ToModel(category);

        return ProductModel.ToModel(existed, categoryModel);
    }

    private string NormalizeCode(string code)
    {
        return code?.Trim().ToUpperInvariant() ?? string.Empty;
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

        _productRepo.SoftDelete(existed);

        await _productRepo.SaveAsync();

        return ProductModel.ToModel(existed);
    }


}