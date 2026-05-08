using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public interface IItineraryDayRepository : IRepository<ItineraryDay>
{
    Task<IEnumerable<ItineraryDay>> GetByTripIdAsync(Guid tripId);
    Task<ItineraryDay?> GetByIdWithActivitiesAsync(Guid id);
}
