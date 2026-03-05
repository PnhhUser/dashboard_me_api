using Microsoft.EntityFrameworkCore;

public partial class MeContext
{
    public DbSet<AccountEntity> Accounts { get; set; } = null!;
    public DbSet<CategoryEntity> Categories { get; set; } = null!;
    public DbSet<ProductEntity> Products { get; set; } = null!;
    public DbSet<ProductImageEntity> ProductImages { get; set; } = null!;
    public DbSet<SupplierEntity> Suppliers { get; set; } = null!;
    public DbSet<PurchaseOrderEntity> PurchaseOrders { get; set; } = null!;
    public DbSet<PurchaseOrderItemEntity> PurchaseOrderItems { get; set; } = null!;
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
}
