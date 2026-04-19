using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories;

namespace TravelPlannerAPI.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _repository;

    public ExpenseService(IExpenseRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Expense>> GetAllAsync(Guid? tripId)
    {
        if (tripId.HasValue)
            return await _repository.GetByTripIdAsync(tripId.Value);
        return await _repository.GetAllAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public async Task<Expense> CreateAsync(Expense expense)
    {
        expense.Id = Guid.NewGuid();
        expense.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(expense);
        await _repository.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense?> UpdateAsync(Guid id, Expense expense)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Category = expense.Category;
        existing.Amount = expense.Amount;
        existing.Currency = expense.Currency;
        existing.Description = expense.Description;
        existing.Date = expense.Date;
        existing.PaymentMethod = expense.PaymentMethod;

        await _repository.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var expense = await _repository.GetByIdAsync(id);
        if (expense == null) return false;

        _repository.Delete(expense);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> GetTotalByTripIdAsync(Guid tripId) =>
        await _repository.GetTotalByTripIdAsync(tripId);
}
