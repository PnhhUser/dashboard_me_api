using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SupplierEntityConfiguration : IEntityTypeConfiguration<SupplierEntity>
{
       public void Configure(EntityTypeBuilder<SupplierEntity> builder)
       {
              builder.ToTable("suppliers");

              builder.HasKey(x => x.Id);

              builder.Property(x => x.Name)
                     .HasMaxLength(150)
                     .IsRequired();

              builder.Property(x => x.Phone)
                     .HasMaxLength(20)
                     .IsRequired();

              builder.Property(x => x.Address)
                     .HasMaxLength(300)
                     .IsRequired();

              builder.Property(x => x.Email)
                     .HasMaxLength(150)
                     .IsRequired();

              builder.HasIndex(x => x.Phone)
                     .IsUnique();

              builder.Property(x => x.CreatedAt)
              .HasColumnType("datetime");

              builder.Property(x => x.UpdatedAt)
              .HasColumnType("datetime");

              builder.Property(x => x.DeletedAt)
              .HasColumnType("datetime");
       }
}