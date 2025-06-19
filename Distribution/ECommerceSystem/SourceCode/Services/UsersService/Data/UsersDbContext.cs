using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace UsersService.Data;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // تكوين جدول المستخدمين
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            
            // إنشاء فهارس
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // إضافة بيانات تجريبية
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@example.com",
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
                Username = "user1",
                Email = "user1@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                FullName = "أحمد محمد",
                PhoneNumber = "+966507654321",
                Address = "جدة، المملكة العربية السعودية",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 3,
                Username = "user2",
                Email = "user2@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                FullName = "فاطمة علي",
                PhoneNumber = "+966509876543",
                Address = "الدمام، المملكة العربية السعودية",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );
    }
}
