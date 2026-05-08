using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public class ItineraryDayRepository : IItineraryDayRepository
{
    private readonly ApplicationDbContext _context;

    public ItineraryDayRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ItineraryDay>> GetAllAsync() =>
        await _context.ItineraryDays.ToListAsync();

    public async Task<ItineraryDay?> GetByIdAsync(Guid id) =>
        await _context.ItineraryDays.FindAsync(id);

    public async Task<ItineraryDay?> GetByIdWithActivitiesAsync(Guid id) =>
        await _context.ItineraryDays
            .Include(d => d.Activities.OrderBy(a => a.SortOrder))
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<ItineraryDay>> GetByTripIdAsync(Guid tripId) =>
        await _context.ItineraryDays
            .Include(d => d.Activities.OrderBy(a => a.SortOrder))
            .Where(d => d.TripId == tripId)
            .OrderBy(d => d.DayNumber)
            .ToListAsync();

    public async Task AddAsync(ItineraryDay entity) =>
        await _context.ItineraryDays.AddAsync(entity);

    public void Delete(ItineraryDay entity) =>
        _context.ItineraryDays.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
