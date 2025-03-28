using Microsoft.EntityFrameworkCore;
using ProductService.Data.Context;
using ProductService.Data.Models;

namespace ProductService.Services
{
    public class SubcategoryService : ISubcategoriesService
    {
        private readonly ApplicationDbContext _context;

        public SubcategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subcategory>> GetSubcategoriesAsync()
        {
            return await _context.Subcategories.ToListAsync();
        }
    }
}
