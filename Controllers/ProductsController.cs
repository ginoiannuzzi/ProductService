using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Services;

namespace ProductService.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var product = await _productsService.GetProductByIdAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", id);
                return NotFound(new { message = "Product not found." });
            }
            return Ok(product);
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { message = "Invalid product data." });

            var createdProduct = await _productsService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedProduct"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] Product updatedProduct)
        {
            var product = await _productsService.UpdateProductAsync(id, updatedProduct);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for update.", id);
                return NotFound(new { message = "Product not found." });
            }

            return Ok(product);
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var success = await _productsService.DeleteProductAsync(id);
            if (!success)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for deletion.", id);
                return NotFound(new { message = "Product not found." });
            }

            return NoContent();
        }

        /// <summary>
        /// List products
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="productCode"></param>
        /// <returns></returns>
        [HttpGet("list")]
        public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? productCode = null)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { Message = "Page and pageSize must be greater than zero." });
            }

            var products = await _productsService.GetProductsAsync(productCode, page, pageSize);

            int totalCount = products.Count;
            int totalPages = totalCount / pageSize + (totalCount % pageSize == 0 ? 0 : 1);

            var paginatedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new PaginatedResponse()
            {
                CurrentPage = page,
                Items = paginatedProducts,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            });
        }
    }
}
