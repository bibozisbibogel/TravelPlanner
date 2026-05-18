using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories;

namespace TravelPlannerAPI.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<User?> RegisterAsync(RegisterRequest request)
    {
        if (await _repository.GetByEmailAsync(request.Email) != null)
            return null;

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(user);
        await _repository.SaveChangesAsync();
        return user;
    }

    public async Task<User?> LoginAsync(string email, string password)
    {
        var user = await _repository.GetByEmailAsync(email);
        if (user == null) return null;

        // Support legacy plain-text demo passwords (migrated on first login)
        if (user.PasswordHash.StartsWith("$2"))
        {
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;
        }
        else
        {
            if (user.PasswordHash != password)
                return null;
            // Upgrade to BCrypt on first login
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.UpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
        }

        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<User?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public async Task<User?> UpdateAvatarAsync(Guid id, string avatarUrl)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;
        user.AvatarUrl = avatarUrl;
        user.UpdatedAt = DateTime.UtcNow;
        await _repository.SaveChangesAsync();
        return user;
    }

    public async Task<User?> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user == null) return null;

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Phone = request.Phone;
        user.AvatarUrl = request.AvatarUrl;
        user.PreferredCurrency = request.PreferredCurrency;
        user.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
        return user;
    }
}
