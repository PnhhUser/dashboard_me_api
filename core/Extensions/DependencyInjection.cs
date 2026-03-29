namespace Core.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IProductImageService, ProductImageService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        // services.AddScoped<IFileService, FileService>();
        services.AddScoped<IFileService, R2FileService>();

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IAccountRepo, AccountRepo>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<IProductRepo, ProductRepo>();
        services.AddScoped<IProductImageRepo, ProductImageRepo>();
        services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
        services.AddScoped<IPurchaseOrderRepo, PurchaseOrderRepo>();
        services.AddScoped<IPurchaseOrderItemRepo, PurchaseOrderItemRepo>();


        return services;
    }
}
