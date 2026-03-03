using Core.Errors;
using Core.Exceptions;

/// <summary>
/// Service for managing products
/// </summary>
public class ProductService : IProductService, IProductImageService
{
    private readonly IProductRepo _productRepo;
    private readonly ICategoryRepo _categoryRepo;

    private readonly IProductImageRepo _productImageRepo;

    private readonly IWebHostEnvironment _environment;

    public ProductService(
        IProductRepo productRepo,
        ICategoryRepo categoryRepo,
        IProductImageRepo productImageRepo,
        IWebHostEnvironment environment)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
        _productImageRepo = productImageRepo;
        _environment = environment;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of all active products</returns>
    public async Task<IReadOnlyList<ProductModel>> GetAllAsync()
    {
        var products = await _productRepo.GetAllAsync();

        var result = new List<ProductModel>();

        foreach (var product in products)
        {
            var category = await _categoryRepo.GetByIdAsync(product.CategoryId);

            var images = await _productImageRepo.GetByProductIdAsync(product.Id);

            var model = ProductModel.ToModel(
                product,
                category != null ? CategoryModel.ToModel(category) : null
            );

            model.Images = images
                .OrderBy(x => x.DisplayOrder)
                .Select(x => ProductImageModel.ToModel(x))
                .ToList();

            result.Add(model);
        }

        return result;
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
            throw new AppException(ErrorCode.NotFound, ErrorMessage.ProductNotFound);

        var category = await _categoryRepo.GetByIdAsync(product.CategoryId);

        if (category == null)
            throw new AppException(ErrorCode.NotFound, ErrorMessage.CategoryNotFound);

        var images = await _productImageRepo.GetByProductIdAsync(id);

        var model = ProductModel.ToModel(product, CategoryModel.ToModel(category));

        model.Images = images
            .OrderBy(x => x.DisplayOrder)
            .Select(x => ProductImageModel.ToModel(x))
            .ToList();

        return model;
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
        var normalizedCode = dto.Code.ToUpperInvariant();
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

        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            existed.Name = dto.Name;
        }

        if (!string.IsNullOrWhiteSpace(dto.Description))
        {
            existed.Description = dto.Description;
        }

        var duplicateCode = await _productRepo.GetByCodeAsync(dto.Code.ToUpperInvariant());

        if (duplicateCode != null && duplicateCode.Id != existed.Id)
        {
            throw new AppException(
                    ErrorCode.ValidationError,
                    ErrorMessage.ProductCodeAlreadyExists
                );
        }
        else
        {
            existed.Code = dto.Code.ToUpperInvariant();
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

        _productRepo.SoftDeleteAsync(existed);

        await _productRepo.SaveAsync();

        return ProductModel.ToModel(existed);
    }

    public async Task UploadImagesAsync(UploadProductImagesDto dto)
    {
        var product = await _productRepo.GetByIdAsync(dto.ProductId);

        if (product == null)
        {
            throw new AppException(
               ErrorCode.NotFound,
               ErrorMessage.ProductNotFound
           );
        }

        if (dto.Files == null || dto.Files.Count == 0)
        {
            throw new AppException(
                ErrorCode.BadRequest,
                ErrorMessage.NoFilesUploaded
            );
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        const long maxFileSize = 5 * 1024 * 1024; // 5MB

        var imageEntities = new List<ProductImageEntity>();

        var uploadPath = Path.Combine(_environment.WebRootPath, "uploads");

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var maxOrder = await _productImageRepo.GetMaxDisplayOrderAsync(dto.ProductId);

        int displayOrder = maxOrder + 1;

        foreach (var file in dto.Files)
        {
            if (file.Length == 0)
                continue;

            if (file.Length > maxFileSize)
                throw new AppException(ErrorCode.BadRequest, "File exceeds 5MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new AppException(ErrorCode.BadRequest, "Invalid file format.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            imageEntities.Add(new ProductImageEntity
            {
                ProductId = dto.ProductId,
                ImageUrl = $"/uploads/{fileName}",
                DisplayOrder = displayOrder++,
                IsPrimary = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!imageEntities.Any())
            throw new AppException(ErrorCode.BadRequest, "No valid images.");

        await _productImageRepo.AddRangeAsync(imageEntities);
        await _productImageRepo.SaveAsync();
    }

    public async Task SetThumbnailAsync(int productId, int displayOrder)
    {
        var affected = await _productImageRepo.SetThumbnailByDisplayOrderAsync(productId, displayOrder);

        if (affected == 0)
        {
            throw new AppException(
                ErrorCode.NotFound,
                "Image not found."
            );
        }

        await _productImageRepo.SaveAsync();
    }

    public async Task<IEnumerable<ProductImageModel>> GetImagesByProductId(int productId)
    {
        var entities = await _productImageRepo.GetByProductIdAsync(productId);

        if (entities == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                "Image not found."
            );
        }

        return entities.Select(ProductImageModel.ToModel);
    }

    public async Task<ProductImageModel> GetThumbnailAsync(int productId)
    {
        var entity = await _productImageRepo.GetThumbnailAsync(productId);

        if (entity == null)
        {
            throw new AppException(
                ErrorCode.NotFound,
                "Image not found."
            );
        }

        return ProductImageModel.ToModel(entity);
    }
}