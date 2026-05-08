using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public class AccommodationRepository : IAccommodationRepository
{
    private readonly ApplicationDbContext _context;

    public AccommodationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Accommodation>> GetAllAsync() =>
        await _context.Accommodations.ToListAsync();

    public async Task<Accommodation?> GetByIdAsync(Guid id) =>
        await _context.Accommodations.FindAsync(id);

    public async Task<IEnumerable<Accommodation>> GetByTripIdAsync(Guid tripId) =>
        await _context.Accommodations
            .Where(a => a.TripId == tripId)
            .OrderBy(a => a.CheckIn)
            .ToListAsync();

    public async Task AddAsync(Accommodation entity) =>
        await _context.Accommodations.AddAsync(entity);

    public void Delete(Accommodation entity) =>
        _context.Accommodations.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
