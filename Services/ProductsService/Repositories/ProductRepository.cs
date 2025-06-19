using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using SharedModels;
using SharedModels.DTOs;

namespace ProductsService.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _context;

    public ProductRepository(ProductsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<PagedResult<Product>> GetPagedAsync(int pageNumber, int pageSize, string? category = null, string? searchTerm = null)
    {
        var query = _context.Products.AsQueryable();

        // تطبيق فلتر الفئة
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category.Contains(category));
        }

        // تطبيق البحث
        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public async Task<Product> CreateAsync(Product product)
    {
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
            return null;

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.Category = product.Category;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existingProduct;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
    {
        return await _context.Products
            .Where(p => p.Category.Contains(category))
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
            .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
            .ToListAsync();
    }

    public async Task<bool> UpdateStockAsync(int id, int newStock)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return false;

        product.Stock = newStock;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}
