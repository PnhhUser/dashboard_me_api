namespace Core.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStockService, StockService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IAccountRepo, AccountRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<IStockRepo, StockRepo>();

        return services;
    }
}
