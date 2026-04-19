using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories;

namespace TravelPlannerAPI.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _repository;

    public TripService(ITripRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Trip>> GetAllAsync(Guid? userId) =>
        await _repository.GetAllWithDetailsAsync(userId);

    public async Task<Trip?> GetByIdAsync(Guid id) =>
        await _repository.GetWithFullDetailsAsync(id);

    public async Task<Trip> CreateAsync(Trip trip)
    {
        trip.Id = Guid.NewGuid();
        trip.CreatedAt = DateTime.UtcNow;
        trip.UpdatedAt = DateTime.UtcNow;
        await _repository.AddAsync(trip);
        await _repository.SaveChangesAsync();
        return trip;
    }

    public async Task<Trip?> UpdateAsync(Guid id, Trip trip)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Title = trip.Title;
        existing.DestinationId = trip.DestinationId;
        existing.StartDate = trip.StartDate;
        existing.EndDate = trip.EndDate;
        existing.TravelersCount = trip.TravelersCount;
        existing.TotalBudget = trip.TotalBudget;
        existing.Status = trip.Status;
        existing.Notes = trip.Notes;
        existing.UpdatedAt = DateTime.UtcNow;

        await _repository.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var trip = await _repository.GetByIdAsync(id);
        if (trip == null) return false;

        _repository.Delete(trip);
        await _repository.SaveChangesAsync();
        return true;
    }
}
