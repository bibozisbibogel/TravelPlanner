using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TripsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/trips?userId={userId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Trip>>> GetAll([FromQuery] Guid? userId)
    {
        var query = _context.Trips
            .Include(t => t.Destination)
            .Include(t => t.Expenses)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);

        return await query.OrderByDescending(t => t.StartDate).ToListAsync();
    }

    // GET: api/trips/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Trip>> GetById(Guid id)
    {
        var trip = await _context.Trips
            .Include(t => t.Destination)
            .Include(t => t.ItineraryDays)
                .ThenInclude(d => d.Activities)
            .Include(t => t.Accommodations)
            .Include(t => t.Expenses)
            .Include(t => t.Collaborators)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trip == null)
            return NotFound();

        return trip;
    }

    // POST: api/trips
    [HttpPost]
    public async Task<ActionResult<Trip>> Create(Trip trip)
    {
        trip.Id = Guid.NewGuid();
        trip.CreatedAt = DateTime.UtcNow;
        trip.UpdatedAt = DateTime.UtcNow;

        _context.Trips.Add(trip);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip);
    }

    // PUT: api/trips/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Trip trip)
    {
        if (id != trip.Id)
            return BadRequest();

        var existing = await _context.Trips.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Title = trip.Title;
        existing.DestinationId = trip.DestinationId;
        existing.StartDate = trip.StartDate;
        existing.EndDate = trip.EndDate;
        existing.TravelersCount = trip.TravelersCount;
        existing.TotalBudget = trip.TotalBudget;
        existing.Status = trip.Status;
        existing.Notes = trip.Notes;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    // DELETE: api/trips/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var trip = await _context.Trips.FindAsync(id);
        if (trip == null)
            return NotFound();

        _context.Trips.Remove(trip);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
