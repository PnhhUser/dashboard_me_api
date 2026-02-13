using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class JwtExtension
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtSettings = configuration
                        .GetSection("JwtSettings")
                        .Get<JwtSettings>()
                        ?? throw new Exception("JwtSettings is not configured");

        services.Configure<JwtSettings>(
            configuration.GetSection("JwtSettings"));

        services.AddAuthentication("Bearer")
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.Secret)),

                        ClockSkew = TimeSpan.Zero
                    };
            });

        return services;
    }
}
