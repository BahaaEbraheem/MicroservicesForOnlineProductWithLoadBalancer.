using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthTestController : ControllerBase
{
    private readonly ILogger<AuthTestController> _logger;

    public AuthTestController(ILogger<AuthTestController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// اختبار المصادقة - متاح للجميع
    /// </summary>
    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult PublicEndpoint()
    {
        return Ok(new
        {
            Message = "هذا endpoint متاح للجميع",
            Timestamp = DateTime.UtcNow,
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false
        });
    }

    /// <summary>
    /// اختبار المصادقة - يحتاج JWT token
    /// </summary>
    [HttpGet("protected")]
    [Authorize]
    public IActionResult ProtectedEndpoint()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        return Ok(new
        {
            Message = "تم الوصول بنجاح للـ endpoint المحمي",
            Timestamp = DateTime.UtcNow,
            User = new
            {
                Id = userId,
                Username = username,
                Email = email,
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            }
        });
    }

    /// <summary>
    /// معلومات المستخدم الحالي
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var claims = User.Claims.ToDictionary(c => c.Type, c => c.Value);
        
        return Ok(new
        {
            Success = true,
            Data = new
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Username = User.FindFirst(ClaimTypes.Name)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
                FullName = User.FindFirst("FullName")?.Value,
                IsActive = User.FindFirst("IsActive")?.Value,
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                AuthenticationType = User.Identity?.AuthenticationType,
                AllClaims = claims
            },
            Message = "تم استرداد معلومات المستخدم بنجاح"
        });
    }
}
