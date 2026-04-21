using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _service;

    public WeatherController(IWeatherService service)
    {
        _service = service;
    }

    // GET: api/weather?city=Tokyo
    [HttpGet]
    public async Task<IActionResult> GetForecast([FromQuery] string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return BadRequest("city is required");

        var forecast = await _service.GetForecastAsync(city);
        if (forecast == null)
            return NotFound($"Could not retrieve weather for '{city}'");

        return Ok(forecast);
    }
}
