using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IUserService
{
    Task<ApplicationUser?> RegisterAsync(RegisterRequest request);
    Task<ApplicationUser?> LoginAsync(string email, string password, bool isPersistent = false);
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser?> GetByIdAsync(Guid id);
    Task<ApplicationUser?> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<ApplicationUser?> UpdateAvatarAsync(Guid id, string avatarUrl);
}
