using Microsoft.EntityFrameworkCore;

public partial class MeContext
{
    public DbSet<AccountEntity> Accounts { get; set; } = null!;
}
