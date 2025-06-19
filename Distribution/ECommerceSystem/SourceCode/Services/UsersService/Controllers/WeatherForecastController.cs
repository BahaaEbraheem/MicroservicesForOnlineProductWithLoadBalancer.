using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using UsersService.Services;
using SharedModels.DTOs;

namespace UsersService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// تسجيل مستخدم جديد - لا يحتاج مصادقة
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterUserDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "البيانات المدخلة غير صحيحة",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
            });
        }

        _logger.LogInformation("Registering new user: {Username}", registerDto.Username);
        var result = await _userService.RegisterAsync(registerDto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// تسجيل الدخول - لا يحتاج مصادقة
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "البيانات المدخلة غير صحيحة",
                Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
            });
        }

        _logger.LogInformation("User login attempt: {Username}", loginDto.Username);
        var result = await _userService.LoginAsync(loginDto);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }

    /// <summary>
    /// الحصول على جميع المستخدمين - يحتاج مصادقة
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
    {
        _logger.LogInformation("Getting all users");
        var result = await _userService.GetAllUsersAsync();

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// الحصول على مستخدم بواسطة المعرف
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int id)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", id);
        var result = await _userService.GetUserByIdAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// تحديث مستخدم
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(int id, [FromBody] UserDto userDto)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);
        var result = await _userService.UpdateUserAsync(id, userDto);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// حذف مستخدم
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(int id)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);
        var result = await _userService.DeleteUserAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// تفعيل مستخدم
    /// </summary>
    [HttpPatch("{id}/activate")]
    public async Task<ActionResult<ApiResponse<bool>>> ActivateUser(int id)
    {
        _logger.LogInformation("Activating user with ID: {UserId}", id);
        var result = await _userService.ActivateUserAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// إلغاء تفعيل مستخدم
    /// </summary>
    [HttpPatch("{id}/deactivate")]
    public async Task<ActionResult<ApiResponse<bool>>> DeactivateUser(int id)
    {
        _logger.LogInformation("Deactivating user with ID: {UserId}", id);
        var result = await _userService.DeactivateUserAsync(id);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }

    /// <summary>
    /// التحقق من صحة الرمز المميز
    /// </summary>
    [HttpPost("validate-token")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<bool>>> ValidateToken([FromBody] string token)
    {
        _logger.LogInformation("Validating token");
        var result = await _userService.ValidateTokenAsync(token);

        return Ok(result);
    }

    /// <summary>
    /// الحصول على بيانات المستخدم الحالي - يحتاج مصادقة
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetCurrentUser()
    {
        try
        {
            // الحصول على معرف المستخدم من JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(new ApiResponse<UserDto>
                {
                    Success = false,
                    Message = "رمز المصادقة غير صالح"
                });
            }

            _logger.LogInformation("Getting current user data for user ID: {UserId}", userId);
            var result = await _userService.GetUserByIdAsync(userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user data");
            return BadRequest(new ApiResponse<UserDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد بيانات المستخدم",
                Errors = new List<string> { ex.Message }
            });
        }
    }
}
