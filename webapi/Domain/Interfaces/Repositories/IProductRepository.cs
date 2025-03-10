using webapi.Domain.Entities;

namespace webapi.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(int id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
}
