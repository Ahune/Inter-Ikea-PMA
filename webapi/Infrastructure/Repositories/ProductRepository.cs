using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductManagementApp.Infrastructure.Persistence;
using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Repositories;

namespace webapi.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(AppDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Product product)
    {
        if (ProductExists(product.Id))
        {
            _logger.LogWarning($"Product with id {product.Id} already exists");
            throw new InvalidOperationException($"Product with id {product.Id} already exists");
        }

        _logger.LogInformation("Adding a new product");
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Product added successfully");
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all products");
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Retrieving product with id {id}");
        return await _context.Products
           .Include(p => p.ProductType)
           .Include(p => p.ProductColours)
           .ThenInclude(pc => pc.Colour)
           .FirstOrDefaultAsync(p => p.Id == id);
    }

    private bool ProductExists(int id)
    {
        _logger.LogInformation($"Checking if product with id {id} exists");
        return _context.Products.Any(e => e.Id == id);
    }
}
