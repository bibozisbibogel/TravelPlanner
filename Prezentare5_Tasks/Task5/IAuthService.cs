using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IAuthService
{
    Task<ApplicationUser?> RegisterAsync(RegisterRequest request);
    Task<ApplicationUser?> LoginAsync(string email, string password, bool isPersistent = false);
    Task LogoutAsync();
}
