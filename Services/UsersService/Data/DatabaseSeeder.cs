using SharedModels;
using Microsoft.EntityFrameworkCore;

namespace UsersService.Data;

public static class DatabaseSeeder
{
    public static void SeedDatabase(UsersDbContext context)
    {
        try
        {
            // Check if database exists and create if not found
            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("✅ Users database created for the first time");
            }
            else
            {
                Console.WriteLine("ℹ️ Users database already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error creating database: {ex.Message}");
            return;
        }

        // Check if data already exists
        if (context.Users.Any())
        {
            Console.WriteLine("ℹ️ Seed data already exists");
            return; // Data already exists
        }

        // إضافة المستخدمين التجريبيين
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@ecommerce.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                FullName = "مدير النظام",
                PhoneNumber = "+966501234567",
                Address = "الرياض، المملكة العربية السعودية",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 2,
                Username = "ahmed",
                Email = "ahmed@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("ahmed123"),
                FullName = "أحمد محمد العلي",
                PhoneNumber = "+966507654321",
                Address = "جدة، المملكة العربية السعودية",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 3,
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                FullName = "مستخدم تجريبي",
                PhoneNumber = "+966501122334",
                Address = "الرياض، المملكة العربية السعودية",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        };

        context.Users.AddRange(users);
        context.SaveChanges();

        Console.WriteLine("✅ Users database initialized successfully");
        Console.WriteLine($"📊 Added {users.Count} users");
        Console.WriteLine("👤 Available users:");
        foreach (var user in users)
        {
            Console.WriteLine($"   - {user.Username} / {user.Username}123");
        }
    }
}
