using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetByTripIdAsync(Guid tripId);
    Task<decimal> GetTotalByTripIdAsync(Guid tripId);
}
