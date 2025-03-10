using webapi.Domain.Entities;

namespace webapi.Domain.Interfaces.Services;

public interface IProductService
{
    Task<Product> GetProductDetailsAsync(int id);
    Task<IEnumerable<Product>> GetProductListAsync();
    Task AddProductAsync(Product product);
    Task<bool> IsProductNameUniqueAsync(string name);
}
