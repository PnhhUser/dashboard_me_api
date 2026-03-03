public class PurchaseOrderItemEntity : BaseEntity
{
    public int Quantity { get; set; }

    public decimal UnitCost { get; set; }

    public int PurchaseOrderId { get; set; }
    public PurchaseOrderEntity PurchaseOrder { get; set; } = null!;

    public int ProductId { get; set; }
    public ProductEntity Product { get; set; } = null!;
}