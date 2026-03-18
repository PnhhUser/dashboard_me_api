using Core.Errors;
using Core.Exceptions;

public class ProductImageService : IProductImageService
{
    private readonly IProductRepo _productRepo;
    private readonly IProductImageRepo _productImageRepo;
    private readonly IFileService _fileService;
    private readonly MeContext _dbContext;

    public ProductImageService(
        IProductRepo productRepo,
        IProductImageRepo productImageRepo,
        IFileService fileService,
        MeContext dbContext)
    {
        _productRepo = productRepo;
        _productImageRepo = productImageRepo;
        _fileService = fileService;
        _dbContext = dbContext;
    }


    public async Task UploadImagesAsync(int productId, List<IFormFile> files)
    {
        var product = await _productRepo.GetByIdAsync(productId)
            ?? throw new AppException(ErrorCode.NotFound, ErrorMessage.ProductNotFound);

        if (files == null || !files.Any())
            throw new AppException(ErrorCode.BadRequest, ErrorMessage.NoFilesUploaded);

        var maxOrder = await _productImageRepo.GetMaxDisplayOrderAsync(productId);
        int displayOrder = maxOrder + 1;

        var imageEntities = new List<ProductImageEntity>();

        foreach (var file in files)
        {
            var imgUrl = await _fileService.SaveFileAsync(file, "uploads");

            imageEntities.Add(new ProductImageEntity
            {
                ProductId = productId,
                ImageUrl = imgUrl,
                DisplayOrder = displayOrder++,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _productImageRepo.AddRangeAsync(imageEntities);
        await _productImageRepo.SaveAsync();
    }


    public async Task ChangeImagesAsync(int productId, List<ProductImageUploadDto> images)
    {
        if (images == null || !images.Any())
            throw new AppException(ErrorCode.BadRequest, "No images provided.");

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            foreach (var item in images)
            {
                await ChangeImageAsync(productId, item.DisplayOrder, item.File);
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task ChangeImageAsync(int productId, int displayOrder, IFormFile file)
    {
        var image = await _productImageRepo
            .GetByProductAndOrderAsync(productId, displayOrder)
            ?? throw new AppException(ErrorCode.NotFound, ErrorMessage.ImageNotFound);

        var oldUrl = image.ImageUrl;

        var newUrl = await _fileService.SaveFileAsync(file, "uploads");

        image.ImageUrl = newUrl;
        await _productImageRepo.SaveAsync();

        await _fileService.DeleteFileAsync(oldUrl);
    }

    public async Task RemoveImage(int productId, int displayOrder)
    {
        var image = await _productImageRepo
            .GetByProductAndOrderAsync(productId, displayOrder)
            ?? throw new AppException(ErrorCode.NotFound, ErrorMessage.ImageNotFound);

        _productImageRepo.Remove(image);

        var reorder = await _productImageRepo
            .GetImagesGreaterThanOrderAsync(productId, displayOrder);

        foreach (var img in reorder)
            img.DisplayOrder--;

        await _productImageRepo.SaveAsync();

        await _fileService.DeleteFileAsync(image.ImageUrl);
    }


    public async Task SetThumbnailAsync(int productId, int displayOrder)
    {
        var affected = await _productImageRepo
            .SetThumbnailByDisplayOrderAsync(productId, displayOrder);

        if (affected == 0)
            throw new AppException(ErrorCode.NotFound, ErrorMessage.ImageNotFound);

        await _productImageRepo.SaveAsync();
    }

    public async Task<IEnumerable<ProductImageModel>> GetImagesByProductId(int productId)
    {
        var entities = await _productImageRepo.GetByProductIdAsync(productId);

        if (!entities.Any())
            throw new AppException(ErrorCode.NotFound, ErrorMessage.ImageNotFound);

        return entities.Select(ProductImageModel.ToModel);
    }

    public async Task<ProductImageModel> GetThumbnailAsync(int productId)
    {
        var image = await _productImageRepo.GetThumbnailAsync(productId);

        if (image == null)
            throw new AppException(ErrorCode.NotFound, ErrorMessage.ImageNotFound);

        return ProductImageModel.ToModel(image);
    }
}