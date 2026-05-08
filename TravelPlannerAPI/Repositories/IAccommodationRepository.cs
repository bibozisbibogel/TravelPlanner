using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public interface IAccommodationRepository : IRepository<Accommodation>
{
    Task<IEnumerable<Accommodation>> GetByTripIdAsync(Guid tripId);
}
