using Microsoft.EntityFrameworkCore;
using ProductService.Data.Context;
using ProductService.Data.Models;

namespace ProductService.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext _context;

        public ProductsService(ApplicationDbContext context)
        {
            _context = context;
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
