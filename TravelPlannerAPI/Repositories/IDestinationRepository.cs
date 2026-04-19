using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public interface IDestinationRepository : IRepository<Destination>
{
    Task<bool> ExistsAsync(string name, string country, Guid? excludeId = null);
}
