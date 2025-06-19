using SharedModels.DTOs;

namespace UsersService.Services;

public interface IUserService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
    Task<ApiResponse<UserDto>> GetUserByIdAsync(int id);
    Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UserDto userDto);
    Task<ApiResponse<bool>> DeleteUserAsync(int id);
    Task<ApiResponse<bool>> ActivateUserAsync(int id);
    Task<ApiResponse<bool>> DeactivateUserAsync(int id);
    Task<ApiResponse<bool>> ValidateTokenAsync(string token);
}
