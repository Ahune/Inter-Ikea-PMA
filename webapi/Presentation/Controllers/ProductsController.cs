using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webapi.Application.Interfaces;
using webapi.Presentation.DTOs.Requests;
using webapi.Presentation.DTOs.Responses;

namespace webapi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductAppService productAppService, ILogger<ProductsController> logger)
        {
            _productAppService = productAppService;
            _logger = logger;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListResponse>>> GetProducts()
        {
            try
            {
                var products = await _productAppService.GetProductListAsync();
                return Ok(products);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound("No products found."); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product list.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productAppService.GetProductDetailsAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound("Id not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching product with ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDetailsResponse>> PostProduct(ProductRequest productRequest)
        {
            try
            {
                await _productAppService.AddProductAsync(productRequest);
                return StatusCode(201, "Product created successfully.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest("Name already exists.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
