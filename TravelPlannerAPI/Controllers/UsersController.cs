using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            return Conflict(new { message = "Email already in use." });

        user.Id = Guid.NewGuid();
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || user.PasswordHash != request.Password)
            return Unauthorized(new { message = "Invalid email or password." });

        return user;
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return NotFound();

        return user;
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, User user)
    {
        if (id != user.Id)
            return BadRequest();

        var existing = await _context.Users.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.FirstName = user.FirstName;
        existing.LastName = user.LastName;
        existing.Phone = user.Phone;
        existing.AvatarUrl = user.AvatarUrl;
        existing.PreferredCurrency = user.PreferredCurrency;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
