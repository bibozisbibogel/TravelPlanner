using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccommodationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AccommodationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/accommodations?tripId={tripId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Accommodation>>> GetAll([FromQuery] Guid? tripId)
    {
        var query = _context.Accommodations.AsQueryable();

        if (tripId.HasValue)
            query = query.Where(a => a.TripId == tripId.Value);

        return await query.OrderBy(a => a.CheckIn).ToListAsync();
    }

    // GET: api/accommodations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Accommodation>> GetById(Guid id)
    {
        var accommodation = await _context.Accommodations.FindAsync(id);
        if (accommodation == null)
            return NotFound();

        return accommodation;
    }

    // POST: api/accommodations
    [HttpPost]
    public async Task<ActionResult<Accommodation>> Create(Accommodation accommodation)
    {
        accommodation.Id = Guid.NewGuid();
        accommodation.CreatedAt = DateTime.UtcNow;

        _context.Accommodations.Add(accommodation);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = accommodation.Id }, accommodation);
    }

    // PUT: api/accommodations/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Accommodation accommodation)
    {
        if (id != accommodation.Id)
            return BadRequest();

        var existing = await _context.Accommodations.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Name = accommodation.Name;
        existing.Address = accommodation.Address;
        existing.CheckIn = accommodation.CheckIn;
        existing.CheckOut = accommodation.CheckOut;
        existing.PricePerNight = accommodation.PricePerNight;
        existing.BookingReference = accommodation.BookingReference;
        existing.AccommodationType = accommodation.AccommodationType;
        existing.Rating = accommodation.Rating;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    // DELETE: api/accommodations/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var accommodation = await _context.Accommodations.FindAsync(id);
        if (accommodation == null)
            return NotFound();

        _context.Accommodations.Remove(accommodation);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
