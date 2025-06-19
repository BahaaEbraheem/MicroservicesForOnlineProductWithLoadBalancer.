using SharedModels;
using Microsoft.EntityFrameworkCore;

namespace OrdersService.Data;

public static class DatabaseSeeder
{
    public static void SeedDatabase(OrdersDbContext context)
    {
        // لا حاجة لإنشاء قاعدة البيانات هنا لأنها تم إنشاؤها في Program.cs

        // Check if data already exists
        if (context.Orders.Any())
        {
            Console.WriteLine("ℹ️ Seed data already exists");
            return; // Data already exists
        }

        // إضافة الطلبات التجريبية بدون تعيين Id يدوياً
        var orders = new List<Order>
{
    new Order
    {
        UserId = 1,
        TotalAmount = 1244.00m,
        Status = OrderStatus.Delivered,
        OrderDate = DateTime.UtcNow.AddDays(-5),
        ShippingAddress = "الرياض، المملكة العربية السعودية"
    },
    new Order
    {
        UserId = 2,
        TotalAmount = 70.00m,
        Status = OrderStatus.Processing,
        OrderDate = DateTime.UtcNow.AddDays(-2),
        ShippingAddress = "جدة، المملكة العربية السعودية"
    },
    new Order
    {
        UserId = 3,
        TotalAmount = 399.00m,
        Status = OrderStatus.Pending,
        OrderDate = DateTime.UtcNow.AddDays(-1),
        ShippingAddress = "الرياض، المملكة العربية السعودية"
    }
};

        context.Orders.AddRange(orders);
        context.SaveChanges(); // احفظ الطلبات أولاً عشان تتولد لهم Id تلقائيًا

        // الآن بعد ما الطلبات اتخزنت وتولدت لهم Ids، ممكن تستخدمهم في عناصر الطلبات
        var orderItems = new List<OrderItem>
{
    // عناصر الطلب الأول
    new OrderItem
    {
        OrderId = orders[0].Id,  // استخدام الـ Id المتولد تلقائيًا
        ProductId = 1,
        Quantity = 1,
        UnitPrice = 1200.00m
    },
    new OrderItem
    {
        OrderId = orders[0].Id,
        ProductId = 5,
        Quantity = 1,
        UnitPrice = 45.00m
    },

    // عناصر الطلب الثاني
    new OrderItem
    {
        OrderId = orders[1].Id,
        ProductId = 5,
        Quantity = 1,
        UnitPrice = 45.00m
    },
    new OrderItem
    {
        OrderId = orders[1].Id,
        ProductId = 6,
        Quantity = 1,
        UnitPrice = 55.00m
    },

    // عناصر الطلب الثالث
    new OrderItem
    {
        OrderId = orders[2].Id,
        ProductId = 4,
        Quantity = 1,
        UnitPrice = 399.00m
    }
};

        context.OrderItems.AddRange(orderItems);
        context.SaveChanges();


        Console.WriteLine("✅ Orders database initialized successfully");
        Console.WriteLine($"📊 Added {orders.Count} orders");
        Console.WriteLine($"📦 Added {orderItems.Count} order items");

        foreach (var order in orders)
        {
            var itemsCount = orderItems.Count(oi => oi.OrderId == order.Id);
            Console.WriteLine($"   - Order #{order.Id}: {order.Status} - {itemsCount} items - {order.TotalAmount:C}");
        }
    }
}
