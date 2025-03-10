using AutoMapper;
using webapi.Application.Interfaces;
using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Services;
using webapi.Presentation.DTOs.Requests;
using webapi.Presentation.DTOs.Responses;

namespace webapi.Application.Services;

public class ProductAppService : IProductAppService
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductAppService> _logger;

    public ProductAppService(IProductService productService, IMapper mapper, ILogger<ProductAppService> logger)
    {
        _productService = productService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task AddProductAsync(ProductRequest productRequest)
    {
        _logger.LogInformation("AddProductAsync called");
        try
        {
            var isNameUnique = await _productService.IsProductNameUniqueAsync(productRequest.Name);
            if (!isNameUnique)
            {
                _logger.LogWarning($"Product with name '{productRequest.Name}' already exists.");
                throw new InvalidOperationException($"Product with name '{productRequest.Name}' already exists.");
            }

            var product = _mapper.Map<Product>(productRequest);
            await _productService.AddProductAsync(product);
            _logger.LogInformation("Product added successfully");
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, $"Error adding product: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error adding product");
            throw new ApplicationException("An unexpected error occurred while adding the product."); // Wrap it in a more general exception
        }
    }

    public async Task<ProductDetailsResponse> GetProductDetailsAsync(int id)
    {
        _logger.LogInformation($"GetProductDetailsAsync called with id: {id}");

        try
        {
            var product = await _productService.GetProductDetailsAsync(id);

            if (product == null)
            {
                _logger.LogWarning($"Product with id {id} not found.");
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            var response = _mapper.Map<ProductDetailsResponse>(product);
            _logger.LogInformation($"Product details retrieved successfully for id: {id}");
            return response;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogError(ex, $"Error retrieving product details for id: {id}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error retrieving product details for id: {id}");
            throw new ApplicationException("An unexpected error occurred while retrieving the product details."); // Wrap in general exception
        }
    }

    public async Task<IEnumerable<ProductListResponse>> GetProductListAsync()
    {
        _logger.LogInformation("GetProductListAsync called");

        try
        {
            var products = await _productService.GetProductListAsync();

            if (products == null || !products.Any())
            {
                _logger.LogWarning("No products found.");
                throw new InvalidOperationException("No products found.");
            }

            var response = _mapper.Map<IEnumerable<ProductListResponse>>(products);
            _logger.LogInformation("Product list retrieved successfully");
            return response;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "No products found");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error retrieving product list");
            throw new ApplicationException("An unexpected error occurred while retrieving the product list.");
        }
    }
}


