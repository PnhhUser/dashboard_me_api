using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

public partial class MeContext : DbContext
{
    public MeContext(DbContextOptions<MeContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MeContext).Assembly
        );

        // global filter for soft‑delete: prevent DeletedAt != null from appearing in queries
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var deletedAt = Expression.PropertyOrField(parameter, nameof(BaseEntity.DeletedAt));
                var nullConstant = Expression.Constant(null);
                var body = Expression.Equal(deletedAt, nullConstant);

                var lambda = Expression.Lambda(body, parameter);
                entityType.SetQueryFilter(lambda);
            }
        }

        AccountSeed.Seed(modelBuilder);
    }
}
