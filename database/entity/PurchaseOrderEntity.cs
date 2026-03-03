public class PurchaseOrderEntity : BaseEntity
{
    public required string Code { get; set; }

    public DateTime OrderDate { get; set; }

    public string? Note { get; set; }

    public required int SupplierId { get; set; }
    public SupplierEntity Supplier { get; set; } = null!;

    public ICollection<PurchaseOrderItemEntity> OrderItems { get; set; } = new List<PurchaseOrderItemEntity>();
}