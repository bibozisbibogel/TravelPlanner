namespace TravelPlannerAPI.Dtos;

public record WeatherForecastDto(
    string City,
    string Country,
    List<WeatherDayDto> Days
);

public record WeatherDayDto(
    string Date,
    double TempMin,
    double TempMax,
    double TempAvg,
    string Description,
    string Icon,
    int Humidity,
    double WindSpeed
);
