using System.ComponentModel.DataAnnotations;

public class PurchaseOrderModel
{
    public int Id { get; set; }

    public required string Code { get; set; }

    public DateTime OrderDate { get; set; }

    public string? Note { get; set; }

    public List<PurchaseOrderItemModel>? PurchaseOrderItemModels { get; set; }

    public SupplierModel? SupplierModel { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}



public class CreatePurchaseOrderDTO
{
    [Required(ErrorMessage = "SupplierId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be greater than 0")]
    public int SupplierId { get; set; }

    [Required(ErrorMessage = "OrderDate is required")]
    public DateTime OrderDate { get; set; }

    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
    public string? Note { get; set; }
}

public class UpdatePurchaseOrderDTO
{
    [Required(ErrorMessage = "Id is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0")]
    public int Id { get; set; }

    [Required(ErrorMessage = "SupplierId is required")]
    [Range(1, int.MaxValue, ErrorMessage = "SupplierId must be greater than 0")]
    public int SupplierId { get; set; }

    [Required(ErrorMessage = "OrderDate is required")]
    public DateTime OrderDate { get; set; }

    [StringLength(500, ErrorMessage = "Note cannot exceed 500 characters")]
    public string? Note { get; set; }
}