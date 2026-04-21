using TravelPlannerAPI.Dtos;

namespace TravelPlannerAPI.Services;

public interface ISearchService
{
    Task<List<FlightResultDto>> SearchFlightsAsync(string from, string to, string departDate, string? returnDate);
    Task<List<HotelResultDto>> SearchHotelsAsync(string location, string checkIn, string checkOut);
    Task<List<PlaceResultDto>> SearchRestaurantsAsync(string location);
    Task<List<PlaceResultDto>> SearchAttractionsAsync(string location);
}
