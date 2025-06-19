using ProductsService.Repositories;
using SharedModels;
using SharedModels.DTOs;

namespace ProductsService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProductsAsync()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = products.Select(MapToDto);
            
            return new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Data = productDtos,
                Message = "تم استرداد المنتجات بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد المنتجات",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "المنتج غير موجود"
                };
            }

            return new ApiResponse<ProductDto>
            {
                Success = true,
                Data = MapToDto(product),
                Message = "تم استرداد المنتج بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد المنتج",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<PagedResult<ProductDto>>> GetProductsPagedAsync(int pageNumber, int pageSize, string? category = null, string? searchTerm = null)
    {
        try
        {
            var pagedResult = await _productRepository.GetPagedAsync(pageNumber, pageSize, category, searchTerm);
            var productDtos = pagedResult.Items.Select(MapToDto).ToList();
            
            var result = new PagedResult<ProductDto>
            {
                Items = productDtos,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };

            return new ApiResponse<PagedResult<ProductDto>>
            {
                Success = true,
                Data = result,
                Message = "تم استرداد المنتجات بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PagedResult<ProductDto>>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد المنتجات",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> CreateProductAsync(CreateProductDto createProductDto)
    {
        try
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
                Category = createProductDto.Category
            };

            var createdProduct = await _productRepository.CreateAsync(product);
            
            return new ApiResponse<ProductDto>
            {
                Success = true,
                Data = MapToDto(createdProduct),
                Message = "تم إنشاء المنتج بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء إنشاء المنتج",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<ProductDto>> UpdateProductAsync(int id, UpdateProductDto updateProductDto)
    {
        try
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return new ApiResponse<ProductDto>
                {
                    Success = false,
                    Message = "المنتج غير موجود"
                };
            }

            // تحديث الخصائص المحددة فقط
            if (!string.IsNullOrEmpty(updateProductDto.Name))
                existingProduct.Name = updateProductDto.Name;
            
            if (!string.IsNullOrEmpty(updateProductDto.Description))
                existingProduct.Description = updateProductDto.Description;
            
            if (updateProductDto.Price.HasValue)
                existingProduct.Price = updateProductDto.Price.Value;
            
            if (updateProductDto.Stock.HasValue)
                existingProduct.Stock = updateProductDto.Stock.Value;
            
            if (!string.IsNullOrEmpty(updateProductDto.Category))
                existingProduct.Category = updateProductDto.Category;

            var updatedProduct = await _productRepository.UpdateAsync(id, existingProduct);
            
            return new ApiResponse<ProductDto>
            {
                Success = true,
                Data = MapToDto(updatedProduct!),
                Message = "تم تحديث المنتج بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء تحديث المنتج",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var result = await _productRepository.DeleteAsync(id);
            if (!result)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "المنتج غير موجود"
                };
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "تم حذف المنتج بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء حذف المنتج",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetProductsByCategoryAsync(string category)
    {
        try
        {
            var products = await _productRepository.GetByCategoryAsync(category);
            var productDtos = products.Select(MapToDto);
            
            return new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Data = productDtos,
                Message = "تم استرداد المنتجات بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد المنتجات",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<ProductDto>>> SearchProductsAsync(string searchTerm)
    {
        try
        {
            var products = await _productRepository.SearchAsync(searchTerm);
            var productDtos = products.Select(MapToDto);
            
            return new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = true,
                Data = productDtos,
                Message = "تم البحث في المنتجات بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = false,
                Message = "حدث خطأ أثناء البحث في المنتجات",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<bool>> UpdateProductStockAsync(int id, int newStock)
    {
        try
        {
            var result = await _productRepository.UpdateStockAsync(id, newStock);
            if (!result)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "المنتج غير موجود"
                };
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "تم تحديث المخزون بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء تحديث المخزون",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            Category = product.Category
        };
    }
}
