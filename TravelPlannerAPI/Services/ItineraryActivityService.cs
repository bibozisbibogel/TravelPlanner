using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories;

namespace TravelPlannerAPI.Services;

public class ItineraryActivityService : IItineraryActivityService
{
    private readonly IItineraryActivityRepository _repository;

    public ItineraryActivityService(IItineraryActivityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ItineraryActivity>> GetAllAsync(Guid? dayId)
    {
        if (dayId.HasValue)
            return await _repository.GetByDayIdAsync(dayId.Value);
        return await _repository.GetAllAsync();
    }

    public async Task<ItineraryActivity?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public async Task<ItineraryActivity> CreateAsync(ItineraryActivity activity)
    {
        activity.Id = Guid.NewGuid();
        activity.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(activity);
        await _repository.SaveChangesAsync();
        return activity;
    }

    public async Task<ItineraryActivity?> UpdateAsync(Guid id, ItineraryActivity activity)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Time = activity.Time;
        existing.Title = activity.Title;
        existing.Description = activity.Description;
        existing.Location = activity.Location;
        existing.Category = activity.Category;
        existing.EstimatedCost = activity.EstimatedCost;
        existing.SortOrder = activity.SortOrder;

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
