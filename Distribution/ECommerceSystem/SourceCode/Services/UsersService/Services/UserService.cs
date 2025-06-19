using UsersService.Repositories;
using SharedModels;
using SharedModels.DTOs;

namespace UsersService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterUserDto registerDto)
    {
        try
        {
            // التحقق من وجود اسم المستخدم
            if (await _userRepository.UsernameExistsAsync(registerDto.Username))
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "اسم المستخدم موجود بالفعل"
                };
            }

            // التحقق من وجود البريد الإلكتروني
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "البريد الإلكتروني موجود بالفعل"
                };
            }

            var user = new User
            {
                Username = registerDto.Username,
                Email = registerDto.Email,
                PasswordHash = registerDto.Password, // سيتم تشفيرها في Repository
                FullName = registerDto.FullName,
                PhoneNumber = registerDto.PhoneNumber,
                Address = registerDto.Address,
                IsActive = true
            };

            var createdUser = await _userRepository.CreateAsync(user);
            var token = _jwtService.GenerateToken(createdUser);

            var authResponse = new AuthResponseDto
            {
                Token = token,
                User = MapToDto(createdUser),
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            return new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Data = authResponse,
                Message = "تم تسجيل المستخدم بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء تسجيل المستخدم",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            if (!await _userRepository.ValidateCredentialsAsync(loginDto.Username, loginDto.Password))
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "اسم المستخدم أو كلمة المرور غير صحيحة"
                };
            }

            var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
            if (user == null || !user.IsActive)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "المستخدم غير نشط أو غير موجود"
                };
            }

            var token = _jwtService.GenerateToken(user);

            var authResponse = new AuthResponseDto
            {
                Token = token,
                User = MapToDto(user),
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            return new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Data = authResponse,
                Message = "تم تسجيل الدخول بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء تسجيل الدخول",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<IEnumerable<UserDto>>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = users.Select(MapToDto);

            return new ApiResponse<IEnumerable<UserDto>>
            {
                Success = true,
                Data = userDtos,
                Message = "تم استرداد المستخدمين بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<UserDto>>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد المستخدمين",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<UserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return new ApiResponse<UserDto>
                {
                    Success = false,
                    Message = "المستخدم غير موجود"
                };
            }

            return new ApiResponse<UserDto>
            {
                Success = true,
                Data = MapToDto(user),
                Message = "تم استرداد المستخدم بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء استرداد المستخدم",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<UserDto>> UpdateUserAsync(int id, UserDto userDto)
    {
        try
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return new ApiResponse<UserDto>
                {
                    Success = false,
                    Message = "المستخدم غير موجود"
                };
            }

            existingUser.Username = userDto.Username;
            existingUser.Email = userDto.Email;
            existingUser.FullName = userDto.FullName;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.Address = userDto.Address;
            existingUser.IsActive = userDto.IsActive;

            var updatedUser = await _userRepository.UpdateAsync(id, existingUser);

            return new ApiResponse<UserDto>
            {
                Success = true,
                Data = MapToDto(updatedUser!),
                Message = "تم تحديث المستخدم بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<UserDto>
            {
                Success = false,
                Message = "حدث خطأ أثناء تحديث المستخدم",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
    {
        try
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "المستخدم غير موجود"
                };
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "تم حذف المستخدم بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء حذف المستخدم",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<bool>> ActivateUserAsync(int id)
    {
        try
        {
            var result = await _userRepository.ActivateUserAsync(id);
            if (!result)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "المستخدم غير موجود"
                };
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "تم تفعيل المستخدم بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء تفعيل المستخدم",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<bool>> DeactivateUserAsync(int id)
    {
        try
        {
            var result = await _userRepository.DeactivateUserAsync(id);
            if (!result)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "المستخدم غير موجود"
                };
            }

            return new ApiResponse<bool>
            {
                Success = true,
                Data = true,
                Message = "تم إلغاء تفعيل المستخدم بنجاح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء إلغاء تفعيل المستخدم",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<bool>> ValidateTokenAsync(string token)
    {
        try
        {
            var isValid = _jwtService.ValidateToken(token);
            return new ApiResponse<bool>
            {
                Success = true,
                Data = isValid,
                Message = isValid ? "الرمز المميز صالح" : "الرمز المميز غير صالح"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "حدث خطأ أثناء التحقق من الرمز المميز",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            IsActive = user.IsActive
        };
    }
}
