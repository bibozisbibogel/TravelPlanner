using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IUserService
{
    Task<IEnumerable<ApplicationUser>> GetAllAsync();
    Task<ApplicationUser?> GetByIdAsync(Guid id);
    Task<ApplicationUser?> UpdateAsync(Guid id, UpdateUserRequest request);
    Task<ApplicationUser?> UpdateAvatarAsync(Guid id, string avatarUrl);
}
