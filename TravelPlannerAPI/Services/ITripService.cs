using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface ITripService
{
    Task<IEnumerable<Trip>> GetAllAsync(Guid? userId);
    Task<Trip?> GetByIdAsync(Guid id);
    Task<Trip> CreateAsync(Trip trip);
    Task<Trip?> UpdateAsync(Guid id, Trip trip);
    Task<bool> DeleteAsync(Guid id);
}
