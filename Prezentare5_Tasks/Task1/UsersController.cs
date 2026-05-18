using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Dtos;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(RegisterRequest request)
    {
        var user = await _service.RegisterAsync(request);
        if (user == null)
            return Conflict(new { message = "Email already in use." });

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<ActionResult<User>> Login([FromBody] LoginRequest request)
    {
        var user = await _service.LoginAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(user);
    }

    // GET: api/users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    // PUT: api/users/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserRequest request)
    {
        var user = await _service.UpdateAsync(id, request);
        if (user == null) return NotFound();
        return Ok(user);
    }
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
