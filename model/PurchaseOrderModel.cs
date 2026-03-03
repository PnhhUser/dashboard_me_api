public class PurchaseOrderModel
{
    public int Id { get; set; }

    public required string Code { get; set; }

    public DateTime OrderDate { get; set; }

    public string? Note { get; set; }
}