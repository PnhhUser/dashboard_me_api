public class ProductEntity : BaseEntity
{
  public string Name { get; set; } = null!;

  public string Description { get; set; } = null!;

  public decimal Price { get; set; }

  public int Stock { get; set; } = 0;
  public decimal AverageCost { get; set; } = 0;

  public ActiveEnum Active { get; set; } = ActiveEnum.Active;

  public required string Code { get; set; }

  public int CategoryId { get; set; }
  public required CategoryEntity Category { get; set; }

  public ICollection<ProductImageEntity> Images { get; set; } = new List<ProductImageEntity>();

  public ICollection<PurchaseOrderItemEntity> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItemEntity>();
}