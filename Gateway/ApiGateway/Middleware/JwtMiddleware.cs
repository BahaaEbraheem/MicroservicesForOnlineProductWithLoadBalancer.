using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiGateway.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = ExtractTokenFromHeader(context);
        
        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var principal = ValidateToken(token);
                if (principal != null)
                {
                    context.User = principal;
                    
                    // إضافة معلومات المستخدم إلى Headers للخدمات الداخلية
                    var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var username = principal.FindFirst(ClaimTypes.Name)?.Value;
                    var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                    
                    if (!string.IsNullOrEmpty(userId))
                        context.Request.Headers.Add("X-User-Id", userId);
                    if (!string.IsNullOrEmpty(username))
                        context.Request.Headers.Add("X-User-Name", username);
                    if (!string.IsNullOrEmpty(email))
                        context.Request.Headers.Add("X-User-Email", email);
                        
                    _logger.LogInformation("JWT token validated successfully for user: {Username}", username);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("JWT token validation failed: {Error}", ex.Message);
            }
        }

        await _next(context);
    }

    private string? ExtractTokenFromHeader(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }
        return null;
    }

    private ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "MyVeryLongSecretKeyForJWTTokenGeneration123456789";
            var issuer = jwtSettings["Issuer"] ?? "ECommerceSystem";
            var audience = jwtSettings["Audience"] ?? "ECommerceUsers";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
