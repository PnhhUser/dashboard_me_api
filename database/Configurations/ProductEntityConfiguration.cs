using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductEntityConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("products");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.CategoryId);

        builder.HasOne(x => x.Category)
       .WithMany()
       .HasForeignKey(x => x.CategoryId)
       .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.Code)
        .IsUnique();

        builder.Property(x => x.Code)
        .HasMaxLength(100)
        .IsRequired();

        builder.Property(x => x.Name)
        .HasMaxLength(255)
        .IsRequired();

        builder.Property(x => x.Description)
        .HasMaxLength(500);

        builder.Property(x => x.Price)
        .HasPrecision(18, 0);

        builder.Property(x => x.Active)
        .HasConversion<int>()
        .IsRequired();

        builder.Property(x => x.CreatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.UpdatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.DeletedAt)
        .HasColumnType("datetime");
    }
}