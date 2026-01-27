using Microsoft.EntityFrameworkCore;

public partial class MeContext : DbContext
{
    public MeContext(DbContextOptions<MeContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MeContext).Assembly
        );

        AccountSeed.Seed(modelBuilder);
    }
}
