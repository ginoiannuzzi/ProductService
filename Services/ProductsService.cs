using Microsoft.EntityFrameworkCore;
using ProductService.Data.Context;
using ProductService.Data.Models;

namespace ProductService.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(ApplicationDbContext dbContext, ILogger<ProductsService> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found.", id);
                return null;
            }
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }

            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Product {ProductId} created successfully.", product.Id);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Guid id, Product updatedProduct)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for update.", id);
                return null;
            }

            product.Name = updatedProduct.Name;
            product.Description = updatedProduct.Description;
            product.SubcategoryId = updatedProduct.SubcategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Product {ProductId} updated successfully.", id);
            return product;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found for deletion.", id);
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        
            _logger.LogInformation("Product {ProductId} deleted successfully.", id);
            return true;
        }

        public async Task<List<Product>> GetProductsAsync(string productCode, int page, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(productCode))
            {
                query = query.Where(p => p.Ski.Contains(productCode));
            }

            var totalCount = await query.CountAsync();
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Subcategory)
                .ToListAsync();

            return products;
        }

        public async Task<int> GetTotalProductCountAsync(string productCode)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(productCode))
            {
                query = query.Where(p => p.Ski.Contains(productCode));
            }

            return await query.CountAsync();
        }
    }
}
