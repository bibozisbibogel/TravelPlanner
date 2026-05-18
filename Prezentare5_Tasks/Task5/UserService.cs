using Microsoft.AspNetCore.Identity;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync() =>
        await Task.FromResult(_userManager.Users.ToList());

    public async Task<ApplicationUser?> GetByIdAsync(Guid id) =>
        await _userManager.FindByIdAsync(id.ToString());

    public async Task<ApplicationUser?> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        user.FirstName         = request.FirstName;
        user.LastName          = request.LastName;
        user.PhoneNumber       = request.Phone;
        user.AvatarUrl         = request.AvatarUrl;
        user.PreferredCurrency = request.PreferredCurrency;
        user.UpdatedAt         = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);
        return user;
    }

    public async Task<ApplicationUser?> UpdateAvatarAsync(Guid id, string avatarUrl)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        user.AvatarUrl  = avatarUrl;
        user.UpdatedAt  = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);
        return user;
    }
}
