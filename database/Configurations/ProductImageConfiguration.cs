using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageEntity>
{
    public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
    {
        builder.ToTable("product_images");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ProductId);

        builder.Property(x => x.ImageUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.UpdatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.DeletedAt)
        .HasColumnType("datetime");
    }
}