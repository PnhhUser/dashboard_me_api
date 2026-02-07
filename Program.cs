using Core.Middlewares;
using Microsoft.EntityFrameworkCore;
using Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");
var version = ServerVersion.AutoDetect(conn);

// Add database context
builder.Services.AddDbContext<MeContext>(option =>
{
    option.UseMySql(conn, version);
});

// Add dependency injection
builder.Services
    .AddApplication()
    .AddInfrastructure();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add controllers with validation
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

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Dashboard Me API",
        Version = "v1",
        Description = "API for Dashboard Management System"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use custom exception middleware
app.UseMiddleware<ExceptionMiddleware>();

// Use CORS
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

app.Run();
