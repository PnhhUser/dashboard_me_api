using System.ComponentModel.DataAnnotations;

public class ProductImageModel
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string ImageUrl { get; set; } = string.Empty;

    public int DisplayOrder { get; set; } = 0;

    public bool IsPrimary { get; set; } = false;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static ProductImageModel ToModel(ProductImageEntity entity)
    {
        return new ProductImageModel
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            ImageUrl = entity.ImageUrl,
            DisplayOrder = entity.DisplayOrder,
            IsPrimary = entity.IsPrimary,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}

public class UploadProductImagesDto
{
    [Required(ErrorMessage = "ProductId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Files are required.")]
    [MinLength(1, ErrorMessage = "At least one image must be uploaded.")]
    public List<IFormFile> Files { get; set; } = new();
}

public class SetProductThumbnailDto
{
    [Required(ErrorMessage = "ProductId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
    public int ProductId { get; set; }


    [Required(ErrorMessage = "DisplayOrder is required.")]
    public int DisplayOrder { get; set; }
}