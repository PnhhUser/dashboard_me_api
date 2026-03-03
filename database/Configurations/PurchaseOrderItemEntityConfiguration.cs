using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PurchaseOrderItemEntityConfiguration : IEntityTypeConfiguration<PurchaseOrderItemEntity>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderItemEntity> builder)
    {
        builder.ToTable("purchase_order_items");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ProductId);

        builder.HasIndex(x => x.PurchaseOrderId);

        builder.Property(x => x.Quantity)
              .IsRequired();

        builder.Property(x => x.UnitCost)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        builder.HasOne(x => x.Product)
               .WithMany(p => p.PurchaseOrderItems)
               .HasForeignKey(x => x.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.PurchaseOrder)
        .WithMany(p => p.OrderItems)
        .HasForeignKey(x => x.PurchaseOrderId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.CreatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.UpdatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.DeletedAt)
        .HasColumnType("datetime");
    }
}