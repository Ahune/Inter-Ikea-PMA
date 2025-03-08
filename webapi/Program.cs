using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Infrastructure.Persistence;
using Scalar.AspNetCore;
using webapi.Infrastructure.Persistance;

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
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
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