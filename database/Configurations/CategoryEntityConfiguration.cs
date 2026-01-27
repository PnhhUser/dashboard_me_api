using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("categories");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Name)
        .IsUnique();

        builder.Property(x => x.Name)
        .HasMaxLength(255)
        .IsRequired();

        builder.Property(x => x.Description)
        .HasMaxLength(500);

        builder.Property(x => x.CreatedAt)
        .HasColumnType("datetime")
             .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
        .HasColumnType("datetime");

        builder.Property(x => x.DeletedAt)
        .HasColumnType("datetime");
    }
}