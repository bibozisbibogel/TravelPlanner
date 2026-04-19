using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public class TripRepository : ITripRepository
{
    private readonly ApplicationDbContext _context;

    public TripRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trip>> GetAllAsync() =>
        await _context.Trips.ToListAsync();

    public async Task<Trip?> GetByIdAsync(Guid id) =>
        await _context.Trips.FindAsync(id);

    public async Task<IEnumerable<Trip>> GetAllWithDetailsAsync(Guid? userId)
    {
        var query = _context.Trips
            .Include(t => t.Destination)
            .Include(t => t.Expenses)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);

        return await query.OrderByDescending(t => t.StartDate).ToListAsync();
    }

    public async Task<Trip?> GetWithFullDetailsAsync(Guid id) =>
        await _context.Trips
            .Include(t => t.Destination)
            .Include(t => t.ItineraryDays).ThenInclude(d => d.Activities)
            .Include(t => t.Accommodations)
            .Include(t => t.Expenses)
            .Include(t => t.Collaborators)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task AddAsync(Trip entity) =>
        await _context.Trips.AddAsync(entity);

    public void Delete(Trip entity) =>
        _context.Trips.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
