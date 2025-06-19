using System.ComponentModel.DataAnnotations;

namespace SharedModels;

// نموذج المنتج
public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public string Category { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

// نموذج المستخدم
public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}

// نموذج الطلب
public class Order
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public decimal TotalAmount { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public string ShippingAddress { get; set; } = string.Empty;

    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

// عنصر الطلب
public class OrderItem
{
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    public Order? Order { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Quantity * UnitPrice;
}

// نموذج الدفع
public class Payment
{
    public int Id { get; set; }

    [Required]
    public int OrderId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    public PaymentMethod Method { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    public string TransactionId { get; set; } = string.Empty;

    public string PaymentGatewayResponse { get; set; } = string.Empty;
}

// حالات الطلب
public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}

// طرق الدفع
public enum PaymentMethod
{
    CreditCard = 0,
    DebitCard = 1,
    PayPal = 2,
    BankTransfer = 3,
    CashOnDelivery = 4
}

// حالات الدفع
public enum PaymentStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2,
    Refunded = 3
}
