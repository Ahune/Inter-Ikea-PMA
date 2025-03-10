using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using webapi.Application.Interfaces;
using webapi.Presentation.Controllers;
using webapi.Presentation.DTOs.Requests;
using webapi.Presentation.DTOs.Responses;
using Xunit;

namespace webapi.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductAppService> _productAppServiceMock;
        private readonly Mock<ILogger<ProductsController>> _loggerMock;
        private readonly ProductsController _productsController;

        public ProductsControllerTests()
        {
            _productAppServiceMock = new Mock<IProductAppService>();
            _loggerMock = new Mock<ILogger<ProductsController>>();
            _productsController = new ProductsController(_productAppServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOk_WhenProductsExist()
        {
            var products = new List<ProductListResponse>
            {
                new ProductListResponse { Id = 1, Name = "Product 1" }
            };
            _productAppServiceMock.Setup(service => service.GetProductListAsync())
                                  .ReturnsAsync(products);

            var result = await _productsController.GetProducts();

            var actionResult = result.Result as OkObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(200);
            actionResult.Value.Should().BeEquivalentTo(products);
        }

        [Fact]
        public async Task GetProducts_ShouldReturnNotFound_WhenNoProductsExist()
        {
            _productAppServiceMock.Setup(service => service.GetProductListAsync())
                                  .ThrowsAsync(new InvalidOperationException());

            var result = await _productsController.GetProducts();

            var actionResult = result.Result as NotFoundObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(404);
            actionResult.Value.Should().Be("No products found.");
        }

        [Fact]
        public async Task GetProducts_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            _productAppServiceMock.Setup(service => service.GetProductListAsync())
                                  .ThrowsAsync(new Exception("Database error"));

            var result = await _productsController.GetProducts();

            var actionResult = result.Result as ObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(500);
            actionResult.Value.Should().Be("Internal server error.");
        }



        [Fact]
        public async Task GetProduct_ShouldReturnOk_WhenProductExists()
        {
            var product = new ProductDetailsResponse { Id = 1, Name = "Product 1" };
            _productAppServiceMock.Setup(service => service.GetProductDetailsAsync(1))
                                  .ReturnsAsync(product);

            var result = await _productsController.GetProduct(1);

            var actionResult = result as OkObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(200);
            actionResult.Value.Should().BeEquivalentTo(product);
        }

        [Fact]
        public async Task GetProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            _productAppServiceMock.Setup(service => service.GetProductDetailsAsync(1))
                                  .ThrowsAsync(new KeyNotFoundException());

            var result = await _productsController.GetProduct(1);

            var actionResult = result as NotFoundObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(404);
            actionResult.Value.Should().Be("Id not found.");
        }

        [Fact]
        public async Task GetProduct_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            _productAppServiceMock.Setup(service => service.GetProductDetailsAsync(1))
                                  .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _productsController.GetProduct(1);

            var actionResult = result as ObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(500);
            actionResult.Value.Should().Be("Internal server error.");
        }

        [Fact]
        public async Task PostProduct_ShouldReturnCreated_WhenProductIsValid()
        {
            var productRequest = new ProductRequest { Name = "New Product" };
            _productAppServiceMock.Setup(service => service.AddProductAsync(productRequest))
                                  .Returns(Task.CompletedTask);

            var result = await _productsController.PostProduct(productRequest);


            result.Result.Should().BeOfType<ObjectResult>();


            var actionResult = result.Result as ObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(201);
            actionResult.Value.Should().Be("Product created successfully.");
        }

        [Fact]
        public async Task PostProduct_ShouldReturnBadRequest_WhenProductNameAlreadyExists()
        {
            var productRequest = new ProductRequest { Name = "Existing Product" };
            _productAppServiceMock.Setup(service => service.AddProductAsync(productRequest))
                                  .ThrowsAsync(new InvalidOperationException());

            var result = await _productsController.PostProduct(productRequest);

            var actionResult = result.Result as BadRequestObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(400);
            actionResult.Value.Should().Be("Name already exists.");
        }

        [Fact]
        public async Task PostProduct_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            var productRequest = new ProductRequest { Name = "New Product" };
            _productAppServiceMock.Setup(service => service.AddProductAsync(productRequest))
                                  .ThrowsAsync(new Exception("Unexpected error"));

            var result = await _productsController.PostProduct(productRequest);

            var actionResult = result.Result as ObjectResult;
            actionResult.Should().NotBeNull();
            actionResult.StatusCode.Should().Be(500);
            actionResult.Value.Should().Be("Internal server error.");
        }
    }
}
