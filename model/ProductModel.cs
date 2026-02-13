using System.ComponentModel.DataAnnotations;

public class ProductModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public ActiveEnum Active { get; set; } = ActiveEnum.Active;

    public required string Code { get; set; }

    public int CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public static ProductModel ToModel(ProductEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new ProductModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Active = entity.Active,
            Code = entity.Code,
            CategoryId = entity.CategoryId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}

public class CreateProductDTO
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 255 characters")]
    public string Name { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = null!;

    public ActiveEnum Active { get; set; }

    [Required(ErrorMessage = "Product code is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Product code must be between 1 and 100 characters")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be valid")]
    public int CategoryId { get; set; }
}

public class EditProductDTO
{
    [Required(ErrorMessage = "Product ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Product ID must be valid")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Product name must be between 1 and 255 characters")]
    public string Name { get; set; } = null!;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = null!;

    public ActiveEnum Active { get; set; }

    [Required(ErrorMessage = "Product code is required")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Product code must be between 1 and 100 characters")]
    public required string Code { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be valid")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
}