using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IUserService
{
    Task<User?> RegisterAsync(RegisterRequest request);
    Task<User?> LoginAsync(string email, string password);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<User?> UpdateAvatarAsync(Guid id, string avatarUrl);
}
