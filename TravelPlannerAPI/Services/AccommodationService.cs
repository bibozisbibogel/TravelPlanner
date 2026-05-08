using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories;

namespace TravelPlannerAPI.Services;

public class AccommodationService : IAccommodationService
{
    private readonly IAccommodationRepository _repository;

    public AccommodationService(IAccommodationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Accommodation>> GetAllAsync(Guid? tripId)
    {
        if (tripId.HasValue)
            return await _repository.GetByTripIdAsync(tripId.Value);
        return await _repository.GetAllAsync();
    }

    public async Task<Accommodation?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public async Task<Accommodation> CreateAsync(Accommodation accommodation)
    {
        accommodation.Id = Guid.NewGuid();
        accommodation.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(accommodation);
        await _repository.SaveChangesAsync();
        return accommodation;
    }

    public async Task<Accommodation?> UpdateAsync(Guid id, Accommodation accommodation)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Name = accommodation.Name;
        existing.Address = accommodation.Address;
        existing.CheckIn = accommodation.CheckIn;
        existing.CheckOut = accommodation.CheckOut;
        existing.PricePerNight = accommodation.PricePerNight;
        existing.BookingReference = accommodation.BookingReference;
        existing.AccommodationType = accommodation.AccommodationType;
        existing.Rating = accommodation.Rating;

        await _repository.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return false;

        _repository.Delete(existing);
        await _repository.SaveChangesAsync();
        return true;
    }
}
