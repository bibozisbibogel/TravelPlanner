using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccommodationsController : ControllerBase
{
    private readonly IAccommodationService _service;

    public AccommodationsController(IAccommodationService service)
    {
        _service = service;
    }

    // GET: api/accommodations?tripId={tripId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Accommodation>>> GetAll([FromQuery] Guid? tripId)
    {
        var accommodations = await _service.GetAllAsync(tripId);
        return Ok(accommodations);
    }

    // GET: api/accommodations/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Accommodation>> GetById(Guid id)
    {
        var accommodation = await _service.GetByIdAsync(id);
        if (accommodation == null) return NotFound();
        return Ok(accommodation);
    }

    // POST: api/accommodations
    [HttpPost]
    public async Task<ActionResult<Accommodation>> Create(Accommodation accommodation)
    {
        var created = await _service.CreateAsync(accommodation);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/accommodations/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Accommodation accommodation)
    {
        if (id != accommodation.Id) return BadRequest();
        var updated = await _service.UpdateAsync(id, accommodation);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    // DELETE: api/accommodations/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
