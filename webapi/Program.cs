using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Infrastructure.Persistence;
using Scalar.AspNetCore;
using webapi.Application.Interfaces;
using webapi.Application.Mappings;
using webapi.Application.Services;
using webapi.Domain.Interfaces.Repositories;
using webapi.Domain.Interfaces.Services;
using webapi.Domain.Services;
using webapi.Infrastructure.Persistance;
using webapi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.ConfigureServices();

var app = builder.Build();

// Configure Pipeline
app.ConfigurePipeline(app.Environment);

// Seed Data
app.SeedDatabase();

app.Run();

// Extension Methods
public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowMultipleOrigins", builder =>
            {
                builder.WithOrigins(
                    "http://localhost:5173", // npm run dev
                    "http://localhost:4173/") // npm run preview
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IProductAppService, ProductAppService>();
    }

}

public static class WebApplicationExtensions
{
    public static void ConfigurePipeline(this WebApplication app, IWebHostEnvironment environment)
    {

        if (environment.IsDevelopment())
        {
            app.MapScalarApiReference(opt =>
            {
                opt.Title = "Hello developers of IKEA INTER";
                opt.Theme = ScalarTheme.Mars;
                opt.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
            app.MapOpenApi();
        }
        app.UseCors("AllowMultipleOrigins");
        app.UseHttpsRedirection();
        app.MapControllers();
    }

    public static void SeedDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            SeedData.Seed(context);
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred seeding the DB.");
        }
    }
}