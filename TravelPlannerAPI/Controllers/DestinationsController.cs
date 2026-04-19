using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DestinationsController : ControllerBase
{
    private readonly IDestinationService _service;

    public DestinationsController(IDestinationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Destination>>> GetAll()
    {
        var destinations = await _service.GetAllAsync();
        return Ok(destinations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Destination>> GetById(Guid id)
    {
        var destination = await _service.GetByIdAsync(id);
        if (destination == null) return NotFound();
        return Ok(destination);
    }

    [HttpPost]
    public async Task<ActionResult<Destination>> Create(Destination destination)
    {
        var created = await _service.CreateAsync(destination);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Destination destination)
    {
        if (id != destination.Id) return BadRequest();
        var updated = await _service.UpdateAsync(id, destination);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
