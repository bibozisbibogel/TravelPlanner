using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public interface IItineraryActivityRepository : IRepository<ItineraryActivity>
{
    Task<IEnumerable<ItineraryActivity>> GetByDayIdAsync(Guid dayId);
}
