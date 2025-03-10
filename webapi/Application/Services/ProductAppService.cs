using AutoMapper;
using webapi.Application.Interfaces;
using webapi.Domain.Entities;
using webapi.Domain.Interfaces.Services;
using webapi.Presentation.DTOs.Responses;

namespace webapi.Application.Services;

public class ProductAppService : IProductAppService
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;
    public ProductAppService(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task AddProductAsync(Presentation.DTOs.Requests.ProductRequest productRequest)
    {
        var product = _mapper.Map<Product>(productRequest);
        await _productService.AddProductAsync(product);
    }

    public async Task<ProductDetailsResponse> GetProductDetailsAsync(int id)
    {
        var product = await _productService.GetProductDetailsAsync(id);
        return _mapper.Map<ProductDetailsResponse>(product);
    }

    public async Task<IEnumerable<ProductListResponse>> GetProductListAsync()
    {
        var products = await _productService.GetProductListAsync();
        return _mapper.Map<IEnumerable<ProductListResponse>>(products);
    }
}


