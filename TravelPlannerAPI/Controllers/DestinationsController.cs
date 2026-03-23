using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DestinationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DestinationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/destinations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Destination>>> GetAll()
    {
        return await _context.Destinations.ToListAsync();
    }

    // GET: api/destinations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Destination>> GetById(Guid id)
    {
        var destination = await _context.Destinations.FindAsync(id);
        if (destination == null)
            return NotFound();

        return destination;
    }

    // POST: api/destinations
    [HttpPost]
    public async Task<ActionResult<Destination>> Create(Destination destination)
    {
        destination.Id = Guid.NewGuid();
        destination.CreatedAt = DateTime.UtcNow;

        _context.Destinations.Add(destination);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = destination.Id }, destination);
    }

    // PUT: api/destinations/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Destination destination)
    {
        if (id != destination.Id)
            return BadRequest();

        var existing = await _context.Destinations.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Name = destination.Name;
        existing.Country = destination.Country;
        existing.Continent = destination.Continent;
        existing.Description = destination.Description;
        existing.ImageUrl = destination.ImageUrl;
        existing.Category = destination.Category;
        existing.AvgBudgetPerDay = destination.AvgBudgetPerDay;
        existing.Rating = destination.Rating;
        existing.Latitude = destination.Latitude;
        existing.Longitude = destination.Longitude;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/destinations/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var destination = await _context.Destinations.FindAsync(id);
        if (destination == null)
            return NotFound();

        _context.Destinations.Remove(destination);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
