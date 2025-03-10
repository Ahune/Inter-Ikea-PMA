using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Repositories;
using webapi.Domain.Interfaces.Services;

namespace webapi.Domain.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<Product> GetProductDetailsAsync(int id)
    {
        _logger.LogInformation("Fetching details for product with ID: {ProductId}", id);
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID: {ProductId} not found", id);
        }
        return product;
    }

    public async Task<IEnumerable<Product>> GetProductListAsync()
    {
        _logger.LogInformation("Fetching product list");
        return await _productRepository.GetAllAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        _logger.LogInformation("Adding new product with name: {ProductName}", product.Name);
        await _productRepository.AddAsync(product);
        _logger.LogInformation("Product with name: {ProductName} added successfully", product.Name);
        
    }

    public async Task<bool> IsProductNameUniqueAsync(string name)
    {
        _logger.LogInformation("Checking if product name: {ProductName} is unique", name);
        var products = await _productRepository.GetAllAsync();
        return !products.Any(p => p.Name == name);
    }
}