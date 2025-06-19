using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProductsService.Services;
using SharedModels.DTOs;

namespace ProductsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// الحصول على جميع المنتجات - متاح للجميع
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetAllProducts()
    {
        _logger.LogInformation("Getting all products");
        var result = await _productService.GetAllProductsAsync();

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// الحصول على منتج بواسطة المعرف
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> GetProductById(int id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", id);
        var result = await _productService.GetProductByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// الحصول على المنتجات مع التصفح
    /// </summary>
    [HttpGet("paged")]
    public async Task<ActionResult<ApiResponse<PagedResult<ProductDto>>>> GetProductsPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? category = null,
        [FromQuery] string? searchTerm = null)
    {
        _logger.LogInformation("Getting paged products - Page: {PageNumber}, Size: {PageSize}, Category: {Category}, Search: {SearchTerm}",
            pageNumber, pageSize, category, searchTerm);

        var result = await _productService.GetProductsPagedAsync(pageNumber, pageSize, category, searchTerm);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// إنشاء منتج جديد - يحتاج مصادقة
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ApiResponse<ProductDto>>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<ProductDto>
            {
                Success = false,
                Message = "البيانات المدخلة غير صحيحة",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
            });
        }

        _logger.LogInformation("Creating new product: {ProductName}", createProductDto.Name);
        var result = await _productService.CreateProductAsync(createProductDto);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetProductById), new { id = result.Data!.Id }, result);
    }

    /// <summary>
    /// تحديث منتج موجود
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ProductDto>>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        _logger.LogInformation("Updating product with ID: {ProductId}", id);
        var result = await _productService.UpdateProductAsync(id, updateProductDto);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// حذف منتج
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProduct(int id)
    {
        _logger.LogInformation("Deleting product with ID: {ProductId}", id);
        var result = await _productService.DeleteProductAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// الحصول على المنتجات حسب الفئة
    /// </summary>
    [HttpGet("category/{category}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> GetProductsByCategory(string category)
    {
        _logger.LogInformation("Getting products by category: {Category}", category);
        var result = await _productService.GetProductsByCategoryAsync(category);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// البحث في المنتجات
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProductDto>>>> SearchProducts([FromQuery] string searchTerm)
    {
        if (string.IsNullOrEmpty(searchTerm))
        {
            return BadRequest(new ApiResponse<IEnumerable<ProductDto>>
            {
                Success = false,
                Message = "مصطلح البحث مطلوب"
            });
        }

        _logger.LogInformation("Searching products with term: {SearchTerm}", searchTerm);
        var result = await _productService.SearchProductsAsync(searchTerm);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// تحديث مخزون المنتج
    /// </summary>
    [HttpPatch("{id}/stock")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateProductStock(int id, [FromBody] int newStock)
    {
        _logger.LogInformation("Updating stock for product ID: {ProductId} to {NewStock}", id, newStock);
        var result = await _productService.UpdateProductStockAsync(id, newStock);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}
