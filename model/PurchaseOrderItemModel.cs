using System.ComponentModel.DataAnnotations;

public class PurchaseOrderItemModel
{
    public int Quantity { get; set; }

    public decimal UnitCost { get; set; }

    public ProductModel? ProductModel { get; set; }
}

public class CreatePurchaseOrderItemDTO
{
    [Required(ErrorMessage = "ProductId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "UnitCost is required")]
    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "UnitCost must be greater than 0")]
    public decimal UnitCost { get; set; }
}

public class UpdatePurchaseOrderItemDTO
{
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
    public int Id { get; set; }

    [Required(ErrorMessage = "ProductId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "UnitCost is required")]
    [Range(typeof(decimal), "0.01", "999999999", ErrorMessage = "UnitCost must be greater than 0")]
    public decimal UnitCost { get; set; }
}