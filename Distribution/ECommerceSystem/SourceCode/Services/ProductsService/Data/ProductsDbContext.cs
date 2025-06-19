using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace ProductsService.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // تكوين جدول المنتجات
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Category);
        });

        // إضافة بيانات تجريبية
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "لابتوب Dell XPS 13",
                Description = "لابتوب عالي الأداء مع معالج Intel Core i7",
                Price = 1200.00m,
                Stock = 10,
                Category = "إلكترونيات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
                Name = "هاتف iPhone 14",
                Description = "أحدث هاتف ذكي من Apple",
                Price = 999.00m,
                Stock = 25,
                Category = "إلكترونيات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 3,
                Name = "كتاب البرمجة بـ C#",
                Description = "دليل شامل لتعلم البرمجة بلغة C#",
                Price = 45.00m,
                Stock = 50,
                Category = "كتب",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 4,
                Name = "ماوس لاسلكي Logitech",
                Description = "ماوس لاسلكي عالي الدقة",
                Price = 25.00m,
                Stock = 100,
                Category = "إكسسوارات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 5,
                Name = "لوحة مفاتيح ميكانيكية",
                Description = "لوحة مفاتيح ميكانيكية للألعاب",
                Price = 85.00m,
                Stock = 30,
                Category = "إكسسوارات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        );
    }
}
