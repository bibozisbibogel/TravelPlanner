using TravelPlannerAPI.Dtos;

namespace TravelPlannerAPI.Services;

public interface IWeatherService
{
    Task<WeatherForecastDto?> GetForecastAsync(string city);
}
