using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.Application.Interfaces;
using webapi.Domain.Entities;
using webapi.Presentation.DTOs.Requests;
using webapi.Presentation.DTOs.Responses;

namespace webapi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IMapper _mapper;

        public ProductsController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListResponse>>> GetProducts()
        {
            var products = await _productAppService.GetProductListAsync();
            if (products == null || !products.Any())
            {
                return NotFound();
            }
            return Ok(products);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailsResponse>> GetProduct(int id)
        {
            var product = await _productAppService.GetProductDetailsAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDetailsResponse>> PostProduct(ProductRequest productRequest)
        {
            if (productRequest == null)
            {
                return BadRequest("Product data is invalid.");
            }
            try
            {
                await _productAppService.AddProductAsync(productRequest);
                return Created();
            }
            catch (Exception ex)
            {
                // Log the exception (replace with your logging mechanism)
                Console.WriteLine($"Error creating product: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the product.");
            }

        }
    }
}
