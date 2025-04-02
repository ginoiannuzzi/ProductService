using ProductService.Models;

namespace ProductService.Controllers
{
    public class PaginatedResponse
    {
        public List<Product> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}
