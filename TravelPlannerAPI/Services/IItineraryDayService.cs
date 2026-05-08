using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IItineraryDayService
{
    Task<IEnumerable<ItineraryDay>> GetAllAsync(Guid? tripId);
    Task<ItineraryDay?> GetByIdAsync(Guid id);
    Task<ItineraryDay> CreateAsync(ItineraryDay day);
    Task<ItineraryDay?> UpdateAsync(Guid id, ItineraryDay day);
    Task<bool> DeleteAsync(Guid id);
}
