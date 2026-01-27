using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StockEntityConfiguration : IEntityTypeConfiguration<StockEntity>
{
    public void Configure(EntityTypeBuilder<StockEntity> builder)
    {
        builder.ToTable("stocks");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.ProductId);

        builder.Property(x => x.Cost)
        .HasPrecision(18, 0);

        builder.Property(x => x.CreatedAt)
        .HasColumnType("datetime")
             .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.DeletedAt)
        .HasColumnType("datetime");
    }
}