using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrdersService.Data;
using SharedModels;
using SharedModels.DTOs;

namespace OrdersService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrdersDbContext _context;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(OrdersDbContext context, ILogger<OrdersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderDto>>>> GetAllOrders()
    {
        try
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();

            var orderDtos = orders.Select(MapToDto);

            return Ok(new ApiResponse<IEnumerable<OrderDto>>
            {
                Success = true,
                Data = orderDtos,
                Message = "تم استرداد الطلبات بنجاح"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<OrderDto>>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد الطلبات",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
    {
        try
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound(new ApiResponse<OrderDto>
                {
                    Success = false,
                    Message = "الطلب غير موجود"
                });
            }

            return Ok(new ApiResponse<OrderDto>
            {
                Success = true,
                Data = MapToDto(order),
                Message = "تم استرداد الطلب بنجاح"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد الطلب",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        try
        {
            var order = new Order
            {
                UserId = createOrderDto.UserId,
                ShippingAddress = createOrderDto.ShippingAddress,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                OrderItems = createOrderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = 0 // سيتم تحديثه من خدمة المنتجات
                }).ToList()
            };

            // حساب المجموع (في التطبيق الحقيقي، سيتم الحصول على الأسعار من خدمة المنتجات)
            order.TotalAmount = order.OrderItems.Sum(item => item.Quantity * 50); // سعر افتراضي

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, new ApiResponse<OrderDto>
            {
                Success = true,
                Data = MapToDto(order),
                Message = "تم إنشاء الطلب بنجاح"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<OrderDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء إنشاء الطلب",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ApiResponse<bool>>> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
    {
        try
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "الطلب غير موجود"
                });
            }

            order.Status = status;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "تم تحديث حالة الطلب بنجاح"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء تحديث حالة الطلب",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            OrderDate = order.OrderDate,
            ShippingAddress = order.ShippingAddress,
            OrderItems = order.OrderItems?.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = $"منتج {item.ProductId}",
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalPrice = item.TotalPrice
            }).ToList() ?? new List<OrderItemDto>()
        };
    }
}
