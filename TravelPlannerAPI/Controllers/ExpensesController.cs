using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ExpensesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/expenses?tripId={tripId}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Expense>>> GetAll([FromQuery] Guid? tripId)
    {
        var query = _context.Expenses.AsQueryable();

        if (tripId.HasValue)
            query = query.Where(e => e.TripId == tripId.Value);

        return await query.OrderByDescending(e => e.Date).ToListAsync();
    }

    // GET: api/expenses/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Expense>> GetById(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
            return NotFound();

        return expense;
    }

    // POST: api/expenses
    [HttpPost]
    public async Task<ActionResult<Expense>> Create(Expense expense)
    {
        expense.Id = Guid.NewGuid();
        expense.CreatedAt = DateTime.UtcNow;

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = expense.Id }, expense);
    }

    // PUT: api/expenses/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Expense expense)
    {
        if (id != expense.Id)
            return BadRequest();

        var existing = await _context.Expenses.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.Category = expense.Category;
        existing.Amount = expense.Amount;
        existing.Currency = expense.Currency;
        existing.Description = expense.Description;
        existing.Date = expense.Date;
        existing.PaymentMethod = expense.PaymentMethod;

        await _context.SaveChangesAsync();

        return Ok(existing);
    }

    // DELETE: api/expenses/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var expense = await _context.Expenses.FindAsync(id);
        if (expense == null)
            return NotFound();

        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
