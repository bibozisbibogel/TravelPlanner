using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IDestinationService
{
    Task<IEnumerable<Destination>> GetAllAsync();
    Task<Destination?> GetByIdAsync(Guid id);
    Task<Destination> CreateAsync(Destination destination);
    Task<Destination?> UpdateAsync(Guid id, Destination destination);
    Task<bool> DeleteAsync(Guid id);
}
