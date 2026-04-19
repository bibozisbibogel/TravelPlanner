using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public interface ITripRepository : IRepository<Trip>
{
    Task<IEnumerable<Trip>> GetAllWithDetailsAsync(Guid? userId);
    Task<Trip?> GetWithFullDetailsAsync(Guid id);
}
