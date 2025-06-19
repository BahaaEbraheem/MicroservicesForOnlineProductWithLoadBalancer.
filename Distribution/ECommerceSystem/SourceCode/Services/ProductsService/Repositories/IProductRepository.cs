using SharedModels;
using SharedModels.DTOs;

namespace ProductsService.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<PagedResult<Product>> GetPagedAsync(int pageNumber, int pageSize, string? category = null, string? searchTerm = null);
    Task<Product> CreateAsync(Product product);
    Task<Product?> UpdateAsync(int id, Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryAsync(string category);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task<bool> UpdateStockAsync(int id, int newStock);
}
