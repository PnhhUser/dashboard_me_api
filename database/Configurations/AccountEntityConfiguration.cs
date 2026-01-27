using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccountEntityConfiguration
    : IEntityTypeConfiguration<AccountEntity>
{
       public void Configure(EntityTypeBuilder<AccountEntity> builder)
       {
              builder.ToTable("accounts");

              builder.HasKey(x => x.Id);

              builder.HasIndex(x => x.Username)
                     .IsUnique();

              builder.Property(x => x.Username)
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(x => x.PasswordHash)
                     .IsRequired();

              builder.Property(x => x.Active)
                     .HasConversion<int>()
                     .IsRequired();

              builder.Property(x => x.Role)
                     .HasConversion<int>()
                     .IsRequired();

              builder.Property(x => x.IsOnline)
                     .HasDefaultValue(false);

              builder.Property(x => x.CreatedAt)
                     .HasColumnType("datetime")
                     .HasDefaultValueSql("CURRENT_TIMESTAMP");

              builder.Property(x => x.UpdatedAt)
                     .HasColumnType("datetime");

              builder.Property(x => x.DeletedAt)
                     .HasColumnType("datetime");
       }
}
