using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}
