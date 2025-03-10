using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using webapi.Application.Services;
using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Services;
using webapi.Presentation.DTOs.Requests;
using webapi.Presentation.DTOs.Responses;
using Xunit;

namespace webapi.Tests
{
    public class ProductAppServiceTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProductAppService>> _loggerMock;
        private readonly ProductAppService _productAppService;

        public ProductAppServiceTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProductAppService>>();
            _productAppService = new ProductAppService(_productServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProductSuccessfully_WhenProductIsValid()
        {
            var productRequest = new ProductRequest { Name = "Test Product" };
            var product = new Product { Id = 1, Name = "Test Product" };

            _productServiceMock.Setup(service => service.IsProductNameUniqueAsync(productRequest.Name)).ReturnsAsync(true);
            _mapperMock.Setup(mapper => mapper.Map<Product>(productRequest)).Returns(product);
            _productServiceMock.Setup(service => service.AddProductAsync(product)).Returns(Task.CompletedTask);

            await _productAppService.AddProductAsync(productRequest);

            _productServiceMock.Verify(service => service.AddProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task AddProductAsync_ShouldThrowInvalidOperationException_WhenProductNameIsNotUnique()
        {
            var productRequest = new ProductRequest { Name = "Test Product" };

            _productServiceMock.Setup(service => service.IsProductNameUniqueAsync(productRequest.Name)).ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _productAppService.AddProductAsync(productRequest));
        }

        [Fact]
        public async Task AddProductAsync_ShouldThrowApplicationException_WhenUnexpectedErrorOccurs()
        {
            var productRequest = new ProductRequest { Name = "Test Product" };

            _productServiceMock.Setup(service => service.IsProductNameUniqueAsync(productRequest.Name)).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<ApplicationException>(() => _productAppService.AddProductAsync(productRequest));
        }

        [Fact]
        public async Task GetProductDetailsAsync_ShouldReturnProductDetails_WhenIdIsValid()
        {
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product" };
            var productDetailsResponse = new ProductDetailsResponse { Id = productId, Name = "Test Product" };

            _productServiceMock.Setup(service => service.GetProductDetailsAsync(productId)).ReturnsAsync(product);
            _mapperMock.Setup(mapper => mapper.Map<ProductDetailsResponse>(product)).Returns(productDetailsResponse);

            var result = await _productAppService.GetProductDetailsAsync(productId);

            result.Should().NotBeNull();
            result.Id.Should().Be(productId);
        }

        [Fact]
        public async Task GetProductDetailsAsync_ShouldThrowKeyNotFoundException_WhenIdIsInvalid()
        {
            var productId = 1;

            _productServiceMock.Setup(service => service.GetProductDetailsAsync(productId)).ReturnsAsync((Product)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _productAppService.GetProductDetailsAsync(productId));
        }

        [Fact]
        public async Task GetProductDetailsAsync_ShouldThrowApplicationException_WhenUnexpectedErrorOccurs()
        {
            var productId = 1;

            _productServiceMock.Setup(service => service.GetProductDetailsAsync(productId)).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<ApplicationException>(() => _productAppService.GetProductDetailsAsync(productId));
        }

        [Fact]
        public async Task GetProductListAsync_ShouldReturnProductList_WhenProductsExist()
        {
            var products = new List<Product> { new Product { Id = 1, Name = "Test Product" } };
            var productListResponse = new List<ProductListResponse> { new ProductListResponse { Id = 1, Name = "Test Product" } };

            _productServiceMock.Setup(service => service.GetProductListAsync()).ReturnsAsync(products);
            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<ProductListResponse>>(products)).Returns(productListResponse);

            var result = await _productAppService.GetProductListAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetProductListAsync_ShouldThrowInvalidOperationException_WhenNoProductsExist()
        {
            _productServiceMock.Setup(service => service.GetProductListAsync()).ReturnsAsync(new List<Product>());

            await Assert.ThrowsAsync<InvalidOperationException>(() => _productAppService.GetProductListAsync());
        }

        [Fact]
        public async Task GetProductListAsync_ShouldThrowApplicationException_WhenUnexpectedErrorOccurs()
        {
            _productServiceMock.Setup(service => service.GetProductListAsync()).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<ApplicationException>(() => _productAppService.GetProductListAsync());
        }
    }
}