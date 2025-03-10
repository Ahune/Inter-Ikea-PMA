using Moq;
using webapi.Domain.Interfaces.Repositories;
using webapi.Domain.Services;
using Xunit;

namespace ProductManagementApp.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;
    
    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }
    
}
