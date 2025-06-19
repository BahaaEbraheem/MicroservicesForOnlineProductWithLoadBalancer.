using SharedModels;

namespace UsersService.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> ValidateCredentialsAsync(string username, string password);
    Task<bool> ActivateUserAsync(int id);
    Task<bool> DeactivateUserAsync(int id);
}
