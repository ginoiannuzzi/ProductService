using ProductService.Data.Models;

namespace ProductService.Services
{
    public interface ISubcategoriesService
    {
        Task<List<Subcategory>> GetSubcategoriesAsync();
    }
}
