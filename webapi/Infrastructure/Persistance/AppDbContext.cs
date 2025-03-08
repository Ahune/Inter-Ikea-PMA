using Microsoft.EntityFrameworkCore;
using webapi.Domain.Entities;

namespace ProductManagementApp.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<ProductColour> ProductColours { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.Colours)
                .WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "ProductColours",
                    j => j.HasOne<Colour>()
                        .WithMany()
                        .HasForeignKey("ColourId"),
                    j => j.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey("ProductId")
                );
        }
    }
}
