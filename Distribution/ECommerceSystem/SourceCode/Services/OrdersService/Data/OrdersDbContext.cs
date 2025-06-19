using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace OrdersService.Data;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // تكوين جدول الطلبات
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.OrderDate);
        });

        // تكوين جدول عناصر الطلب
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasPrecision(18, 2);

            // العلاقة مع الطلب
            entity.HasOne(e => e.Order)
                  .WithMany(o => o.OrderItems)
                  .HasForeignKey(e => e.OrderId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // إضافة بيانات تجريبية
        modelBuilder.Entity<Order>().HasData(
            new Order
            {
                Id = 1,
                UserId = 1,
                TotalAmount = 1244.00m,
                Status = OrderStatus.Delivered,
                OrderDate = DateTime.UtcNow.AddDays(-5),
                ShippingAddress = "الرياض، المملكة العربية السعودية"
            },
            new Order
            {
                Id = 2,
                UserId = 2,
                TotalAmount = 70.00m,
                Status = OrderStatus.Processing,
                OrderDate = DateTime.UtcNow.AddDays(-2),
                ShippingAddress = "جدة، المملكة العربية السعودية"
            }
        );

        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem
            {
                Id = 1,
                OrderId = 1,
                ProductId = 1,
                Quantity = 1,
                UnitPrice = 1200.00m
            },
            new OrderItem
            {
                Id = 2,
                OrderId = 1,
                ProductId = 2,
                Quantity = 1,
                UnitPrice = 25.00m
            },
            new OrderItem
            {
                Id = 3,
                OrderId = 2,
                ProductId = 3,
                Quantity = 1,
                UnitPrice = 45.00m
            },
            new OrderItem
            {
                Id = 4,
                OrderId = 2,
                ProductId = 4,
                Quantity = 1,
                UnitPrice = 25.00m
            }
        );
    }
}
