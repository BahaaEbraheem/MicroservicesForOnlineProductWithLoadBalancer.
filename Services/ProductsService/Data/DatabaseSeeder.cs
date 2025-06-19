using SharedModels;
using Microsoft.EntityFrameworkCore;

namespace ProductsService.Data;

public static class DatabaseSeeder
{
    public static void SeedDatabase(ProductsDbContext context)
    {
        try
        {
            // Check if database exists and create if not found
            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("✅ Products database created for the first time");
            }
            else
            {
                Console.WriteLine("ℹ️ Products database already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating database: {ex.Message}");
            return;
        }

        // Check if data already exists
        if (context.Products.Any())
        {
            Console.WriteLine("ℹ️ Seed data already exists");
            return; // Data already exists
        }

        // إضافة المنتجات التجريبية
        var products = new List<Product>
        {
            // إلكترونيات
            new Product
            {
                Name = "لابتوب Dell XPS 13",
                Description = "لابتوب عالي الأداء مع معالج Intel Core i7",
                Price = 1200.00m,
                Stock = 15,
                Category = "إلكترونيات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "هاتف iPhone 14 Pro",
                Description = "أحدث هاتف ذكي من Apple",
                Price = 999.00m,
                Stock = 25,
                Category = "إلكترونيات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "سماعات AirPods Pro",
                Description = "سماعات لاسلكية مع إلغاء الضوضاء",
                Price = 249.00m,
                Stock = 30,
                Category = "إلكترونيات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "ساعة Apple Watch",
                Description = "ساعة ذكية مع مراقبة الصحة",
                Price = 399.00m,
                Stock = 20,
                Category = "إلكترونيات",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // كتب
            new Product
            {
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
                Name = "كتاب تطوير الويب",
                Description = "تعلم تطوير تطبيقات الويب",
                Price = 55.00m,
                Stock = 35,
                Category = "كتب",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // ملابس
            new Product
            {
                Name = "قميص قطني",
                Description = "قميص رجالي قطني عالي الجودة",
                Price = 75.00m,
                Stock = 60,
                Category = "ملابس",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "فستان صيفي",
                Description = "فستان نسائي أنيق للصيف",
                Price = 120.00m,
                Stock = 25,
                Category = "ملابس",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();

        Console.WriteLine("✅ Products database initialized successfully");
        Console.WriteLine($"📊 Added {products.Count} products");
        var categories = products.GroupBy(p => p.Category);
        foreach (var category in categories)
        {
            Console.WriteLine($"   📂 {category.Key}: {category.Count()} products");
        }
    }
}
