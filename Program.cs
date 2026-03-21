using Core.Middlewares;
using Microsoft.EntityFrameworkCore;
using Core.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 🔥 Logging (debug Railway)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// 🔥 Connection string
var conn = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");

var version = ServerVersion.AutoDetect(conn);

// 🔥 DB Context
builder.Services.AddDbContext<MeContext>(option =>
{
    option.UseMySql(conn, version);
});

// 🔥 JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// 🔥 DI
builder.Services
    .AddApplication()
    .AddInfrastructure();


// 🔥🔥🔥 CORS (fix toàn bộ lỗi)
// var allowedOrigins = builder.Configuration
//     .GetSection("Cors:AllowedOrigins")
//     .Get<string[]>() ?? Array.Empty<string>();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAngular", policy =>
//     {
//         policy
//             .SetIsOriginAllowed(origin =>
//                  // cho phép config từ env
//                  allowedOrigins.Any(o => origin.StartsWith(o))

//                 // cho phép localhost dev
//                 || origin.Contains("localhost")

//                 // cho phép Railway frontend
//                 || origin.Contains("railway.app")

//                 // dev tunnel
//                 || origin.EndsWith("devtunnels.ms")
//             )
//             .AllowAnyHeader()
//             .AllowAnyMethod()
//             .AllowCredentials();
//     });
// });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy
            .WithOrigins("https://dashmetest.netlify.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// 🔥 Controller + validation
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = string.Join(", ", context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new
            {
                success = false,
                error = new
                {
                    code = "VALIDATION_ERROR",
                    message = errors
                }
            });
        };
    });


// 🔥 Swagger (bật cả production cho test)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();


// 🔥🔥🔥 AUTO MIGRATE (QUAN TRỌNG NHẤT)
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<MeContext>();
        db.Database.Migrate();
        Console.WriteLine("✅ Database migrated successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Migration failed: " + ex.Message);
        throw;
    }
}


// 🔥 Pipeline

// bật swagger cả production (test API Railway)
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();