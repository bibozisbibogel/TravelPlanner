using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public UsersController(IAuthService authService, IUserService userService,
        UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
    {
        _authService = authService;
        _userService = userService;
        _userManager = userManager;
        _env = env;
    }

    // GET: api/users  (Admin only)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        var responses = new List<UserResponse>();
        foreach (var u in users)
            responses.Add(await ToResponseAsync(u));

        return Ok(responses);
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterAsync(request);
            if (user == null)
                return Conflict(new { message = "Email already in use." });

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, await ToResponseAsync(user));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.LoginAsync(request.Email, request.Password, request.RememberMe);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(await ToResponseAsync(user));
    }

    // POST: api/users/logout
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return NoContent();
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetById(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(await ToResponseAsync(user));
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var user = await _userService.UpdateAsync(id, request);
        if (user == null) return NotFound();
        return Ok(await ToResponseAsync(user));
    }

    // POST: api/users/{id}/avatar
    [HttpPost("{id}/avatar")]
    [Authorize]
    public async Task<IActionResult> UploadAvatar(Guid id, IFormFile file)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        if (!allowed.Contains(ext))
            return BadRequest(new { message = "Only image files are allowed (jpg, png, gif, webp)." });

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest(new { message = "File size must be under 5 MB." });

        var uploadsDir = Path.Combine(
            _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
            "uploads", "avatars");
        Directory.CreateDirectory(uploadsDir);

        foreach (var old in Directory.GetFiles(uploadsDir, $"{id}.*"))
            System.IO.File.Delete(old);

        var fileName = $"{id}{ext}";
        using (var stream = new FileStream(Path.Combine(uploadsDir, fileName), FileMode.Create))
            await file.CopyToAsync(stream);

        var relativeUrl = $"/uploads/avatars/{fileName}";
        await _userService.UpdateAvatarAsync(id, relativeUrl);

        return Ok(new { avatarUrl = $"{Request.Scheme}://{Request.Host}{relativeUrl}" });
    }

    private async Task<UserResponse> ToResponseAsync(ApplicationUser u)
    {
        var roles = await _userManager.GetRolesAsync(u);
        return new UserResponse
        {
            Id                 = u.Id,
            FirstName          = u.FirstName,
            LastName           = u.LastName,
            Email              = u.Email!,
            Phone              = u.PhoneNumber,
            AvatarUrl          = u.AvatarUrl,
            PreferredCurrency  = u.PreferredCurrency,
            Role               = roles.FirstOrDefault() ?? "User",
            CreatedAt          = u.CreatedAt
        };
    }
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; } = false;
}
