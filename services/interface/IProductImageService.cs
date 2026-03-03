public interface IProductImageService
{
    Task UploadImagesAsync(UploadProductImagesDto dto);

    Task SetThumbnailAsync(int productId, int displayOrder);

    Task<IEnumerable<ProductImageModel>> GetImagesByProductId(int productId);

    Task<ProductImageModel> GetThumbnailAsync(int productId);
}