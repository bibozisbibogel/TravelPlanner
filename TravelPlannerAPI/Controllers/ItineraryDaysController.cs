using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItineraryDaysController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ItineraryDaysController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/itinerarydays?tripId={tripId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItineraryDay>>> GetAll([FromQuery] Guid? tripId)
    {
        var query = _context.ItineraryDays
            .Include(d => d.Activities.OrderBy(a => a.SortOrder))
            .AsQueryable();

        if (tripId.HasValue)
            query = query.Where(d => d.TripId == tripId.Value);

        return await query.OrderBy(d => d.DayNumber).ToListAsync();
    }

    // GET: api/itinerarydays/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItineraryDay>> GetById(Guid id)
    {
        var day = await _context.ItineraryDays
            .Include(d => d.Activities.OrderBy(a => a.SortOrder))
            .FirstOrDefaultAsync(d => d.Id == id);

        if (day == null)
            return NotFound();

        return day;
    }

    // POST: api/itinerarydays
    [HttpPost]
    public async Task<ActionResult<ItineraryDay>> Create(ItineraryDay day)
    {
        day.Id = Guid.NewGuid();
        day.CreatedAt = DateTime.UtcNow;

        _context.ItineraryDays.Add(day);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = day.Id }, day);
    }

    // PUT: api/itinerarydays/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ItineraryDay day)
    {
        if (id != day.Id)
            return BadRequest();

        var existing = await _context.ItineraryDays.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.DayNumber = day.DayNumber;
        existing.Date = day.Date;
        existing.Title = day.Title;
        existing.Notes = day.Notes;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    // DELETE: api/itinerarydays/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var day = await _context.ItineraryDays.FindAsync(id);
        if (day == null)
            return NotFound();

        _context.ItineraryDays.Remove(day);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
