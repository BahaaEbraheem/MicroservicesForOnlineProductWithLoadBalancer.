using Microsoft.EntityFrameworkCore;
using UsersService.Data;
using SharedModels;

namespace UsersService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(int id, User user)
    {
        var existingUser = await _context.Users.FindAsync(id);
        if (existingUser == null)
            return null;

        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.FullName = user.FullName;
        existingUser.PhoneNumber = user.PhoneNumber;
        existingUser.Address = user.Address;
        existingUser.IsActive = user.IsActive;

        // تحديث كلمة المرور فقط إذا تم توفيرها
        if (!string.IsNullOrEmpty(user.PasswordHash))
        {
            existingUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        }

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ValidateCredentialsAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user == null || !user.IsActive)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<bool> ActivateUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        user.IsActive = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;

        user.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
