using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Repositories;
using webapi.Domain.Services;
using Xunit;

namespace ProductManagementApp.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly ProductService _productService;
        private readonly Mock<ILogger<ProductService>> _loggerMock;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _loggerMock = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_productRepositoryMock.Object, _loggerMock.Object);
        }

        private void SetupProductRepositoryGetById(int productId, Product product)
        {
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
        }

        private void SetupProductRepositoryGetAll(List<Product> products)
        {
            _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);
        }

        [Fact]
        public async Task GetProductDetailsAsync_ShouldReturnProduct_WhenProductExists()
        {
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product" };
            SetupProductRepositoryGetById(productId, product);

            var result = await _productService.GetProductDetailsAsync(productId);

            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
            result.Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetProductDetailsAsync_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            var productId = 1;
            SetupProductRepositoryGetById(productId, null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productService.GetProductDetailsAsync(productId));
        }

        [Fact]
        public async Task GetProductListAsync_ShouldReturnProductList_WhenProductsExist()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Product 1" } };
            SetupProductRepositoryGetAll(products);

            var result = await _productService.GetProductListAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Product 1");
        }

        [Fact]
        public async Task GetProductListAsync_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            SetupProductRepositoryGetAll(new List<Product>());

            var result = await _productService.GetProductListAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProductSuccessfully_WhenProductIsValid()
        {
            var product = new Product { Id = 1, Name = "Test Product" };
            _productRepositoryMock.Setup(repo => repo.AddAsync(product)).Returns(Task.CompletedTask);

            await _productService.AddProductAsync(product);

            _productRepositoryMock.Verify(repo => repo.AddAsync(product), Times.Once);
        }

        [Fact]
        public async Task AddProductAsync_ShouldThrowInvalidOperationException_WhenRepositoryThrows()
        {
            var product = new Product { Name = "Test Product" };
            _productRepositoryMock.Setup(repo => repo.AddAsync(product)).ThrowsAsync(new InvalidOperationException());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _productService.AddProductAsync(product));
        }

        [Fact]
        public async Task IsProductNameUniqueAsync_ShouldReturnTrue_WhenProductNameIsUnique()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Existing Product" } };
            SetupProductRepositoryGetAll(products);

            var result = await _productService.IsProductNameUniqueAsync("New Product");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsProductNameUniqueAsync_ShouldReturnFalse_WhenProductNameIsNotUnique()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Existing Product" } };
            SetupProductRepositoryGetAll(products);

            var result = await _productService.IsProductNameUniqueAsync("Existing Product");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsProductNameUniqueAsync_ShouldReturnTrue_WhenNoProductsExist()
        {
            SetupProductRepositoryGetAll(new List<Product>());

            var result = await _productService.IsProductNameUniqueAsync("");

            result.Should().BeTrue();
        }
    }
}
