public class ProductImageEntity : BaseEntity
{
    public int ProductId { get; set; }

    public required string ImageUrl { get; set; }

    public int DisplayOrder { get; set; } = 0;

    public bool IsPrimary { get; set; } = false;

    public required ProductEntity Product { get; set; }
}
