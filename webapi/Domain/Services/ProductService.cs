using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Repositories;
using webapi.Domain.Interfaces.Services;

namespace webapi.Domain.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> GetProductDetailsAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetProductListAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        var isNotTaken = await IsProductNameUniqueAsync(product.Name);

        if (isNotTaken)
        {
            await _productRepository.AddAsync(product);
        }
    }

    private async Task<bool> IsProductNameUniqueAsync(string name)
    {
        var products = await _productRepository.GetAllAsync();
        return !products.Any(p => p.Name == name);
    }
}