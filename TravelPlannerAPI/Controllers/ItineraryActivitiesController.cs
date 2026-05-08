using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItineraryActivitiesController : ControllerBase
{
    private readonly IItineraryActivityService _service;

    public ItineraryActivitiesController(IItineraryActivityService service)
    {
        _service = service;
    }

    // GET: api/itineraryactivities?dayId={dayId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItineraryActivity>>> GetAll([FromQuery] Guid? dayId)
    {
        var activities = await _service.GetAllAsync(dayId);
        return Ok(activities);
    }

    // GET: api/itineraryactivities/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ItineraryActivity>> GetById(Guid id)
    {
        var activity = await _service.GetByIdAsync(id);
        if (activity == null) return NotFound();
        return Ok(activity);
    }

    // POST: api/itineraryactivities
    [HttpPost]
    public async Task<ActionResult<ItineraryActivity>> Create(ItineraryActivity activity)
    {
        var created = await _service.CreateAsync(activity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT: api/itineraryactivities/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ItineraryActivity activity)
    {
        if (id != activity.Id) return BadRequest();
        var updated = await _service.UpdateAsync(id, activity);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    // DELETE: api/itineraryactivities/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
