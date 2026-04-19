using TravelPlannerAPI.Models;
using TravelPlannerAPI.Repositories;

namespace TravelPlannerAPI.Services;

public class DestinationService : IDestinationService
{
    private readonly IDestinationRepository _repository;

    public DestinationService(IDestinationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Destination>> GetAllAsync() =>
        await _repository.GetAllAsync();

    public async Task<Destination?> GetByIdAsync(Guid id) =>
        await _repository.GetByIdAsync(id);

    public async Task<Destination> CreateAsync(Destination destination)
    {
        destination.Id = Guid.NewGuid();
        destination.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(destination);
        await _repository.SaveChangesAsync();
        return destination;
    }

    public async Task<Destination?> UpdateAsync(Guid id, Destination destination)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        existing.Name = destination.Name;
        existing.Country = destination.Country;
        existing.Continent = destination.Continent;
        existing.Description = destination.Description;
        existing.ImageUrl = destination.ImageUrl;
        existing.Category = destination.Category;
        existing.AvgBudgetPerDay = destination.AvgBudgetPerDay;
        existing.Rating = destination.Rating;
        existing.Latitude = destination.Latitude;
        existing.Longitude = destination.Longitude;

        await _repository.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var destination = await _repository.GetByIdAsync(id);
        if (destination == null) return false;

        _repository.Delete(destination);
        await _repository.SaveChangesAsync();
        return true;
    }
}
