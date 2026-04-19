using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public class DestinationRepository : IDestinationRepository
{
    private readonly ApplicationDbContext _context;

    public DestinationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Destination>> GetAllAsync() =>
        await _context.Destinations.ToListAsync();

    public async Task<Destination?> GetByIdAsync(Guid id) =>
        await _context.Destinations.FindAsync(id);

    public async Task<bool> ExistsAsync(string name, string country, Guid? excludeId = null)
    {
        var query = _context.Destinations.Where(d => d.Name == name && d.Country == country);
        if (excludeId.HasValue)
            query = query.Where(d => d.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task AddAsync(Destination entity) =>
        await _context.Destinations.AddAsync(entity);

    public void Delete(Destination entity) =>
        _context.Destinations.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
