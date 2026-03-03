public class SupplierEntity : BaseEntity
{
    public required string Name { get; set; }

    public required string Phone { get; set; }

    public required string Address { get; set; }

    public required string Email { get; set; }

    public ICollection<PurchaseOrderEntity> PurchaseOrders { get; set; } = new List<PurchaseOrderEntity>();
}