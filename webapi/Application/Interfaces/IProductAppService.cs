using webapi.Presentation.DTOs.Requests;
using webapi.Presentation.DTOs.Responses;

namespace webapi.Application.Interfaces;

public interface IProductAppService
{
    Task<ProductDetailsResponse> GetProductDetailsAsync(int id);
    Task<IEnumerable<ProductListResponse>> GetProductListAsync();
    Task AddProductAsync(ProductRequest productDto);
}

