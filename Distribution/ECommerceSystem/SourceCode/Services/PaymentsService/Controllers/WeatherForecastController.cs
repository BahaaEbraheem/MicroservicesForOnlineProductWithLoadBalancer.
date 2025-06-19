using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.DTOs;

namespace PaymentsService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(ILogger<PaymentsController> logger)
    {
        _logger = logger;
    }

    [HttpPost("process")]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> ProcessPayment([FromBody] ProcessPaymentDto processPaymentDto)
    {
        try
        {
            // محاكاة معالجة الدفع
            await Task.Delay(1000); // محاكاة وقت المعالجة

            var payment = new Payment
            {
                Id = Random.Shared.Next(1, 1000),
                OrderId = processPaymentDto.OrderId,
                Amount = 100.00m, // سيتم الحصول عليه من خدمة الطلبات
                Method = processPaymentDto.Method,
                Status = Random.Shared.Next(1, 10) > 2 ? PaymentStatus.Completed : PaymentStatus.Failed,
                PaymentDate = DateTime.UtcNow,
                TransactionId = Guid.NewGuid().ToString("N")[..10].ToUpper()
            };

            var paymentDto = new PaymentDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Method = payment.Method,
                Status = payment.Status,
                PaymentDate = payment.PaymentDate,
                TransactionId = payment.TransactionId
            };

            return Ok(new ApiResponse<PaymentDto>
            {
                Success = payment.Status == PaymentStatus.Completed,
                Data = paymentDto,
                Message = payment.Status == PaymentStatus.Completed ? "تم الدفع بنجاح" : "فشل في عملية الدفع"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PaymentDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء معالجة الدفع",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PaymentDto>>> GetPayment(int id)
    {
        try
        {
            // محاكاة استرداد بيانات الدفع
            var paymentDto = new PaymentDto
            {
                Id = id,
                OrderId = 1,
                Amount = 100.00m,
                Method = PaymentMethod.CreditCard,
                Status = PaymentStatus.Completed,
                PaymentDate = DateTime.UtcNow.AddMinutes(-30),
                TransactionId = "TXN123456"
            };

            return Ok(new ApiResponse<PaymentDto>
            {
                Success = true,
                Data = paymentDto,
                Message = "تم استرداد بيانات الدفع بنجاح"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PaymentDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد بيانات الدفع",
                Errors = new List<string> { ex.Message }
            });
        }
    }

    [HttpPost("{id}/refund")]
    public async Task<ActionResult<ApiResponse<bool>>> RefundPayment(int id)
    {
        try
        {
            // محاكاة عملية الاسترداد
            await Task.Delay(500);

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "تم استرداد المبلغ بنجاح"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد المبلغ",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}
