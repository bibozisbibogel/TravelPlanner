using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync() =>
        await _context.Expenses.ToListAsync();

    public async Task<Expense?> GetByIdAsync(Guid id) =>
        await _context.Expenses.FindAsync(id);

    public async Task<IEnumerable<Expense>> GetByTripIdAsync(Guid tripId) =>
        await _context.Expenses
            .Where(e => e.TripId == tripId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();

    public async Task<decimal> GetTotalByTripIdAsync(Guid tripId) =>
        await _context.Expenses
            .Where(e => e.TripId == tripId)
            .SumAsync(e => e.Amount);

    public async Task AddAsync(Expense entity) =>
        await _context.Expenses.AddAsync(entity);

    public void Delete(Expense entity) =>
        _context.Expenses.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
