using SharedModels.DTOs;

namespace ProductsService.Services;

public interface IProductService
{
    Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync();
    Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id);
    Task<ApiResponse<PagedResult<ProductDto>>> GetProductsPagedAsync(int pageNumber, int pageSize, string? category = null, string? searchTerm = null);
    Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto);
    Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
    Task<ApiResponse<bool>> DeleteProductAsync(int id);
    Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(string category);
    Task<ApiResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm);
    Task<ApiResponse<bool>> UpdateProductStockAsync(int id, int newStock);
}
