using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItineraryDaysController : ControllerBase
{
    private readonly IItineraryDayService _service;

    public ItineraryDaysController(IItineraryDayService service)
    {
        _service = service;
    }

    // GET: api/itinerarydays?tripId={tripId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItineraryDay>>> GetAll([FromQuery] Guid? tripId)
    {
        var days = await _service.GetAllAsync(tripId);
        return Ok(days);
    }

    // GET: api/itinerarydays/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItineraryDay>> GetById(Guid id)
    {
        var day = await _service.GetByIdAsync(id);
        if (day == null) return NotFound();
        return Ok(day);
    }

    // POST: api/itinerarydays
    [HttpPost]
    public async Task<ActionResult<ItineraryDay>> Create(ItineraryDay day)
    {
        var created = await _service.CreateAsync(day);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/itinerarydays/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ItineraryDay day)
    {
        if (id != day.Id) return BadRequest();
        var updated = await _service.UpdateAsync(id, day);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    // DELETE: api/itinerarydays/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
