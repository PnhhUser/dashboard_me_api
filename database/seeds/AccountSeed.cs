using Microsoft.EntityFrameworkCore;

public static class AccountSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>().HasData(
            new AccountEntity
            {
                Id = 1,
                Username = "admin",
                PasswordHash = PasswordHasher.Hash("123456"),
                Active = ActiveEnum.Active,
                Role = RoleEnum.Admin,
                IsOnline = false,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}
