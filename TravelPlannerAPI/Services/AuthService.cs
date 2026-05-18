using Microsoft.AspNetCore.Identity;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ApplicationUser?> RegisterAsync(RegisterRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) != null)
            return null;

        var user = new ApplicationUser
        {
            Id        = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName  = request.LastName,
            Email     = request.Email,
            UserName  = request.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join("; ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "User");
        return user;
    }

    public async Task<ApplicationUser?> LoginAsync(string email, string password, bool isPersistent = false)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
        if (!result.Succeeded) return null;

        await _signInManager.SignInAsync(user, isPersistent);
        return user;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
