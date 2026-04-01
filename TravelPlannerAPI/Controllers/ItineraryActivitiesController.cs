using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItineraryActivitiesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ItineraryActivitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/itineraryactivities?dayId={dayId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItineraryActivity>>> GetAll([FromQuery] Guid? dayId)
    {
        var query = _context.ItineraryActivities.AsQueryable();

        if (dayId.HasValue)
            query = query.Where(a => a.ItineraryDayId == dayId.Value);

        return await query.OrderBy(a => a.SortOrder).ToListAsync();
    }

    // GET: api/itineraryactivities/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItineraryActivity>> GetById(Guid id)
    {
        var activity = await _context.ItineraryActivities.FindAsync(id);
        if (activity == null)
            return NotFound();

        return activity;
    }

    // POST: api/itineraryactivities
    [HttpPost]
    public async Task<ActionResult<ItineraryActivity>> Create(ItineraryActivity activity)
    {
        activity.Id = Guid.NewGuid();
        activity.CreatedAt = DateTime.UtcNow;

        _context.ItineraryActivities.Add(activity);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
    }

    // PUT: api/itineraryactivities/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ItineraryActivity activity)
    {
        if (id != activity.Id)
            return BadRequest();

        var existing = await _context.ItineraryActivities.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Time = activity.Time;
        existing.Title = activity.Title;
        existing.Description = activity.Description;
        existing.Location = activity.Location;
        existing.Category = activity.Category;
        existing.EstimatedCost = activity.EstimatedCost;
        existing.SortOrder = activity.SortOrder;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    // DELETE: api/itineraryactivities/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var activity = await _context.ItineraryActivities.FindAsync(id);
        if (activity == null)
            return NotFound();

        _context.ItineraryActivities.Remove(activity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
