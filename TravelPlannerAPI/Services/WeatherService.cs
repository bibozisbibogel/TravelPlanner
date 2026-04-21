using System.Text.Json;
using TravelPlannerAPI.Dtos;

namespace TravelPlannerAPI.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public WeatherService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["OpenWeatherAPI_KEY"] ?? "";
    }

    public async Task<WeatherForecastDto?> GetForecastAsync(string city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/forecast" +
                  $"?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return null;

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();

        var cityEl = json.GetProperty("city");
        var cityName = cityEl.GetProperty("name").GetString() ?? city;
        var country  = cityEl.GetProperty("country").GetString() ?? "";

        // Group by date, prefer the 12:00 reading per day
        var dayMap = new Dictionary<string, JsonElement>();
        foreach (var item in json.GetProperty("list").EnumerateArray())
        {
            var dtTxt = item.GetProperty("dt_txt").GetString()!;
            var date  = dtTxt[..10];
            var time  = dtTxt[11..];
            if (!dayMap.ContainsKey(date) || time == "12:00:00")
                dayMap[date] = item;
        }

        var days = dayMap
            .OrderBy(kv => kv.Key)
            .Take(5)
            .Select(kv =>
            {
                var it   = kv.Value;
                var main = it.GetProperty("main");
                var w    = it.GetProperty("weather")[0];
                var wind = it.GetProperty("wind");
                return new WeatherDayDto(
                    Date:        kv.Key,
                    TempMin:     main.GetProperty("temp_min").GetDouble(),
                    TempMax:     main.GetProperty("temp_max").GetDouble(),
                    TempAvg:     main.GetProperty("temp").GetDouble(),
                    Description: w.GetProperty("description").GetString() ?? "",
                    Icon:        w.GetProperty("icon").GetString() ?? "",
                    Humidity:    main.GetProperty("humidity").GetInt32(),
                    WindSpeed:   wind.GetProperty("speed").GetDouble()
                );
            }).ToList();

        return new WeatherForecastDto(cityName, country, days);
    }
}
