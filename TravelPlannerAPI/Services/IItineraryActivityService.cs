using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IItineraryActivityService
{
    Task<IEnumerable<ItineraryActivity>> GetAllAsync(Guid? dayId);
    Task<ItineraryActivity?> GetByIdAsync(Guid id);
    Task<ItineraryActivity> CreateAsync(ItineraryActivity activity);
    Task<ItineraryActivity?> UpdateAsync(Guid id, ItineraryActivity activity);
    Task<bool> DeleteAsync(Guid id);
}
