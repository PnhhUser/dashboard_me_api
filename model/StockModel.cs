using System.ComponentModel.DataAnnotations;

public class StockModel
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public ProductModel Product { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal Cost { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public static StockModel ToModel(StockEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(entity.Product);

        return new StockModel
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            Product = ProductModel.ToModel(entity.Product),
            Quantity = entity.Quantity,
            Cost = entity.Cost,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
        };
    }
}

public class CreateStockDTO
{
    [Required(ErrorMessage = "Product ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Product ID must be valid")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Cost is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than 0")]
    public decimal Cost { get; set; }
}

public class EditStockDTO
{
    [Required(ErrorMessage = "Stock ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Stock ID must be valid")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Product ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Product ID must be valid")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Cost is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be greater than 0")]
    public decimal Cost { get; set; }
}
