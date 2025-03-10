using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Infrastructure.Persistence;
using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Repositories;

namespace webapi.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductColours)
            .ThenInclude(pc => pc.Colour)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
