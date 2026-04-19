using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IExpenseService
{
    Task<IEnumerable<Expense>> GetAllAsync(Guid? tripId);
    Task<Expense?> GetByIdAsync(Guid id);
    Task<Expense> CreateAsync(Expense expense);
    Task<Expense?> UpdateAsync(Guid id, Expense expense);
    Task<bool> DeleteAsync(Guid id);
    Task<decimal> GetTotalByTripIdAsync(Guid tripId);
}
