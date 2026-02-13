public class ProductEntity : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }

    public ActiveEnum Active { get; set; } = ActiveEnum.Active; // Không hiển thị - hiển thị

    public required string Code { get; set; }

    public int CategoryId { get; set; }
    public required CategoryEntity Category { get; set; }

    public ICollection<ProductImageEntity> Images { get; set; }
      = new List<ProductImageEntity>();
}