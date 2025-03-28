using ProductService.Data.Models;

namespace ProductService.Services
{
    public interface IProductsService
    {
        Task<List<Product>> GetProductsAsync(string productCode, int page, int pageSize);

        Task<int> GetTotalProductCountAsync(string productCode);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Guid id, Product updatedProduct);
        Task<bool> DeleteProductAsync(Guid id);
    }
}
