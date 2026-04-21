using Microsoft.AspNetCore.Mvc;
using TravelPlannerAPI.Services;

namespace TravelPlannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _service;

    public SearchController(ISearchService service)
    {
        _service = service;
    }

    // GET: api/search/flights?from=Bucharest&to=Tokyo&departDate=2025-04-10&returnDate=2025-04-20
    [HttpGet("flights")]
    public async Task<IActionResult> Flights(
        [FromQuery] string from,
        [FromQuery] string to,
        [FromQuery] string departDate,
        [FromQuery] string? returnDate = null)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to) || string.IsNullOrWhiteSpace(departDate))
            return BadRequest("from, to and departDate are required");

        var results = await _service.SearchFlightsAsync(from, to, departDate, returnDate);
        return Ok(results);
    }

    // GET: api/search/hotels?location=Tokyo&checkIn=2025-04-10&checkOut=2025-04-20
    [HttpGet("hotels")]
    public async Task<IActionResult> Hotels(
        [FromQuery] string location,
        [FromQuery] string checkIn,
        [FromQuery] string checkOut)
    {
        if (string.IsNullOrWhiteSpace(location) || string.IsNullOrWhiteSpace(checkIn) || string.IsNullOrWhiteSpace(checkOut))
            return BadRequest("location, checkIn and checkOut are required");

        var results = await _service.SearchHotelsAsync(location, checkIn, checkOut);
        return Ok(results);
    }

    // GET: api/search/restaurants?location=Tokyo
    [HttpGet("restaurants")]
    public async Task<IActionResult> Restaurants([FromQuery] string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return BadRequest("location is required");

        var results = await _service.SearchRestaurantsAsync(location);
        return Ok(results);
    }

    // GET: api/search/places?location=Tokyo
    [HttpGet("places")]
    public async Task<IActionResult> Places([FromQuery] string location)
    {
        if (string.IsNullOrWhiteSpace(location))
            return BadRequest("location is required");

        var results = await _service.SearchAttractionsAsync(location);
        return Ok(results);
    }
}
