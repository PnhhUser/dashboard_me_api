public interface IProductImageService
{
    Task UploadImagesAsync(int productId, List<IFormFile> files);

    Task SetThumbnailAsync(int productId, int displayOrder);

    Task<IEnumerable<ProductImageModel>> GetImagesByProductId(int productId);

    Task<ProductImageModel?> GetThumbnailAsync(int productId);

    Task ChangeImageAsync(int productId, int displayOrder, IFormFile file);

    Task ChangeImagesAsync(int productId, List<ProductImageUploadDto> images);

    Task RemoveImage(int productId, int displayOrder);
}