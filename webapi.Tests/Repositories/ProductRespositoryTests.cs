using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ProductManagementApp.Infrastructure.Persistence;
using webapi.Domain.Entities;
using webapi.Infrastructure.Persistance;
using webapi.Infrastructure.Repositories;
using Xunit;

namespace webapi.Tests
{
    public class ProductRepositoryTests
    {
        private readonly ProductRepository _productRepository;
        private readonly AppDbContext _context;
        private readonly Mock<ILogger<ProductRepository>> _loggerMock;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new AppDbContext(options);

            _loggerMock = new Mock<ILogger<ProductRepository>>();

            _productRepository = new ProductRepository(_context, _loggerMock.Object);

            SeedDatabase(_context);
        }
        private void SeedDatabase(AppDbContext context)
        {
            var productType = new ProductType { Id = 1, Name = "Electronics" };
            context.ProductTypes.Add(productType);

            var colour = new Colour { Id = 1, Name = "Red" };
            context.Colours.Add(colour);

            var productColour = new ProductColour { ColourId = 1, ProductId = 1 };
            context.ProductColours.Add(productColour);

            context.SaveChanges();
        }


        [Fact]
        public async Task AddAsync_ShouldAddProduct_WhenProductIsValid()
        {
            var product = new Product { Id = 1, Name = "Test Product" };

            await _productRepository.AddAsync(product);

            var products = await _context.Products.ToListAsync();
            products.Should().ContainSingle(p => p.Name == "Test Product");
        }

        [Fact]
        public async Task AddAsync_ShouldThrowInvalidOperationException_WhenProductAlreadyExists()
        {
            var product = new Product { Id = 1, Name = "Existing Product" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync(); 

            Func<Task> action = async () => await _productRepository.AddAsync(product);

            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Product with id 1 already exists");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts_WhenProductsExist()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            var result = await _productRepository.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProductWithIncludes_WhenProductExists()
        {
            // Test is linked to eagerloading.
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                ProductTypeId = 1,
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var result = await _productRepository.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Name.Should().Be("Test Product");
            result.ProductType.Name.Should().Be("Electronics"); 
            result.ProductColours.Should().NotBeEmpty();
            result.ProductColours.First().Colour.Name.Should().Be("Red"); 
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            var nonExistingId = 999;
            var result = await _productRepository.GetByIdAsync(nonExistingId);

            result.Should().BeNull();
        }

        [Fact]
        public async Task ProductExists_ShouldReturnTrue_WhenProductExists()
        {
            var product = new Product { Id = 1, Name = "Product 1" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var exists = _context.Products.Any(p => p.Id == 1);

            exists.Should().BeTrue();
        }

        [Fact]
        public void ProductExists_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            var nonExistingId = 999;
            var exists = _context.Products.Any(p => p.Id == nonExistingId);

            exists.Should().BeFalse();
        }
    }
}
