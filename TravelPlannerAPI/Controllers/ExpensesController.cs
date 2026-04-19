using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _service;

    public ExpensesController(IExpenseService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expense>>> GetAll([FromQuery] Guid? tripId)
    {
        var expenses = await _service.GetAllAsync(tripId);
        return Ok(expenses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetById(Guid id)
    {
        var expense = await _service.GetByIdAsync(id);
        if (expense == null) return NotFound();
        return Ok(expense);
    }

    [HttpPost]
    public async Task<ActionResult<Expense>> Create(Expense expense)
    {
        var created = await _service.CreateAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Expense expense)
    {
        if (id != expense.Id) return BadRequest();
        var updated = await _service.UpdateAsync(id, expense);
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
