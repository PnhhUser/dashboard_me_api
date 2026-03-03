using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PurchaseOrderEntityConfiguration : IEntityTypeConfiguration<PurchaseOrderEntity>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderEntity> builder)
    {
        builder.ToTable("purchas_orders");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.SupplierId);

        builder.HasOne(x => x.Supplier)
       .WithMany(s => s.PurchaseOrders)
       .HasForeignKey(x => x.SupplierId)
       .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Code)
        .IsUnique();

        builder.Property(x => x.Code)
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(x => x.Note)
        .HasMaxLength(500);

        builder.Property(x => x.OrderDate)
        .HasColumnType("datetime");

        builder.Property(x => x.CreatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.UpdatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.DeletedAt)
        .HasColumnType("datetime");
    }
}