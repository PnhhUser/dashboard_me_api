using Core.Middlewares;
using Microsoft.EntityFrameworkCore;
using Core.Extensions;

var builder = WebApplication.CreateBuilder(args);


var conn = builder.Configuration.GetConnectionString("Default");
var version = ServerVersion.AutoDetect(conn);

builder.Services.AddDbContext<MeContext>(option =>
{
    option.UseMySql(conn, version);
});

builder.Services
    .AddApplication()
    .AddInfrastructure();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
