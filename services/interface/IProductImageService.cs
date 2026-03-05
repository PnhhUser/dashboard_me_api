public interface IProductImageService
{
    Task UploadImagesAsync(UploadProductImagesDto dto);

    Task SetThumbnailAsync(int productId, int displayOrder);

    Task<IEnumerable<ProductImageModel>> GetImagesByProductId(int productId);

    Task<ProductImageModel> GetThumbnailAsync(int productId);

    Task ChangeImageAsync(int productId, int displayOrder, IFormFile file);

    Task RemoveImage(int productId, int displayOrder);
}