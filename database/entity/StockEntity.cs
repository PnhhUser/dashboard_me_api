public class StockEntity : BaseEntity
{
    public int ProductId { get; set; }
    public required ProductEntity Product { get; set; }

    public int Quantity { get; set; }

    public decimal Cost { get; set; }
}