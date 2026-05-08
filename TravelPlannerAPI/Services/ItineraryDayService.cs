using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories;

namespace TravelPlannerAPI.Services;

public class ItineraryDayService : IItineraryDayService
{
    private readonly IItineraryDayRepository _repository;

    public ItineraryDayService(IItineraryDayRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ItineraryDay>> GetAllAsync(Guid? tripId)
    {
        if (tripId.HasValue)
            return await _repository.GetByTripIdAsync(tripId.Value);
        return await _repository.GetAllAsync();
    }

    public async Task<ItineraryDay?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdWithActivitiesAsync(id);

    public async Task<ItineraryDay> CreateAsync(ItineraryDay day)
    {
        day.Id = Guid.NewGuid();
        day.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(day);
        await _repository.SaveChangesAsync();
        return day;
    }

    public async Task<ItineraryDay?> UpdateAsync(Guid id, ItineraryDay day)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.DayNumber = day.DayNumber;
        existing.Date = day.Date;
        existing.Title = day.Title;
        existing.Notes = day.Notes;

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
