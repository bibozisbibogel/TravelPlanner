namespace TravelPlannerAPI.Dtos;

public record FlightResultDto(
    int Price,
    int TotalDuration,
    string AirlineLogo,
    List<FlightLegDto> Flights
);

public record FlightLegDto(
    string Airline,
    string FlightNumber,
    string DepartureAirport,
    string ArrivalAirport,
    string DepartureTime,
    string ArrivalTime,
    int Duration,
    string TravelClass
);

public record HotelResultDto(
    string Name,
    string Description,
    double Rating,
    int Reviews,
    string PricePerNight,
    string HotelClass,
    List<string> Amenities,
    string Thumbnail,
    string Link
);

public record PlaceResultDto(
    string Title,
    double Rating,
    int Reviews,
    string Type,
    string Address,
    string Hours,
    string Phone,
    string Thumbnail
);
