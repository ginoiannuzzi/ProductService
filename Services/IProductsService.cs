using ProductService.Data.Models;

namespace ProductService.Services
{
    public interface IProductsService
    {
        Task<List<Product>> GetProductsAsync(string productCode, int page, int pageSize);

        Task<int> GetTotalProductCountAsync(string productCode);
    }
}
