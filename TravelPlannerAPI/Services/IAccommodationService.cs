using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Services;

public interface IAccommodationService
{
    Task<IEnumerable<Accommodation>> GetAllAsync(Guid? tripId);
    Task<Accommodation?> GetByIdAsync(Guid id);
    Task<Accommodation> CreateAsync(Accommodation accommodation);
    Task<Accommodation?> UpdateAsync(Guid id, Accommodation accommodation);
    Task<bool> DeleteAsync(Guid id);
}
