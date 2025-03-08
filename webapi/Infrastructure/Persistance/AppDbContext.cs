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

            modelBuilder.Entity<ProductColour>()
                .HasKey(pc => new { pc.ProductId, pc.ColourId });

            modelBuilder.Entity<ProductColour>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColours)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductColour>()
                .HasOne(pc => pc.Colour)
                .WithMany(c => c.ProductColours)
                .HasForeignKey(pc => pc.ColourId);
        }
    }
}
