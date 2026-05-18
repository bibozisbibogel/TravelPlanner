using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IUserService
{
    Task<User?> RegisterAsync(RegisterRequest request);
    Task<User?> LoginAsync(string email, string password);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> UpdateAsync(Guid id, UpdateUserRequest request);
}
