using Microsoft.EntityFrameworkCore;
using TravelPlannerAPI.Data;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public class ItineraryActivityRepository : IItineraryActivityRepository
{
    private readonly ApplicationDbContext _context;

    public ItineraryActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ItineraryActivity>> GetAllAsync() =>
        await _context.ItineraryActivities.ToListAsync();

    public async Task<ItineraryActivity?> GetByIdAsync(Guid id) =>
        await _context.ItineraryActivities.FindAsync(id);

    public async Task<IEnumerable<ItineraryActivity>> GetByDayIdAsync(Guid dayId) =>
        await _context.ItineraryActivities
            .Where(a => a.ItineraryDayId == dayId)
            .OrderBy(a => a.SortOrder)
            .ToListAsync();

    public async Task AddAsync(ItineraryActivity entity) =>
        await _context.ItineraryActivities.AddAsync(entity);

    public void Delete(ItineraryActivity entity) =>
        _context.ItineraryActivities.Remove(entity);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
