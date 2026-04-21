using System.Text.Json;
using TravelPlannerAPI.Dtos;

namespace TravelPlannerAPI.Services;

public class SearchService : ISearchService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public SearchService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _apiKey = config["SERPAPI_KEY"] ?? "";
    }

    // ── Flights ───────────────────────────────���────────────────────────────

    public async Task<List<FlightResultDto>> SearchFlightsAsync(
        string from, string to, string departDate, string? returnDate)
    {
        var url = "https://serpapi.com/search" +
                  $"?engine=google_flights" +
                  $"&departure_id={Uri.EscapeDataString(from)}" +
                  $"&arrival_id={Uri.EscapeDataString(to)}" +
                  $"&outbound_date={departDate}" +
                  (string.IsNullOrEmpty(returnDate) ? "" : $"&return_date={returnDate}") +
                  $"&currency=USD&hl=en&api_key={_apiKey}";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return [];

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        var results = new List<FlightResultDto>();

        foreach (var section in new[] { "best_flights", "other_flights" })
        {
            if (!json.TryGetProperty(section, out var arr)) continue;
            foreach (var item in arr.EnumerateArray().Take(4))
                results.Add(ParseFlight(item));
            if (results.Count >= 6) break;
        }

        return results;
    }

    private static FlightResultDto ParseFlight(JsonElement item)
    {
        var price    = item.TryGetProperty("price", out var p) ? p.GetInt32() : 0;
        var duration = item.TryGetProperty("total_duration", out var d) ? d.GetInt32() : 0;
        var logo     = item.TryGetProperty("airline_logo", out var l) ? l.GetString() ?? "" : "";

        var legs = new List<FlightLegDto>();
        if (item.TryGetProperty("flights", out var flightsArr))
        {
            foreach (var f in flightsArr.EnumerateArray())
            {
                var dep = f.TryGetProperty("departure_airport", out var da) ? da : default;
                var arr = f.TryGetProperty("arrival_airport",   out var aa) ? aa : default;
                legs.Add(new FlightLegDto(
                    Airline:          f.TryGetProperty("airline",       out var al) ? al.GetString() ?? "" : "",
                    FlightNumber:     f.TryGetProperty("flight_number", out var fn) ? fn.GetString() ?? "" : "",
                    DepartureAirport: dep.ValueKind != JsonValueKind.Undefined ? dep.GetProperty("name").GetString() ?? "" : "",
                    ArrivalAirport:   arr.ValueKind != JsonValueKind.Undefined ? arr.GetProperty("name").GetString() ?? "" : "",
                    DepartureTime:    dep.ValueKind != JsonValueKind.Undefined ? dep.GetProperty("time").GetString() ?? "" : "",
                    ArrivalTime:      arr.ValueKind != JsonValueKind.Undefined ? arr.GetProperty("time").GetString() ?? "" : "",
                    Duration:         f.TryGetProperty("duration",      out var dur) ? dur.GetInt32() : 0,
                    TravelClass:      f.TryGetProperty("travel_class",  out var tc)  ? tc.GetString()  ?? "" : ""
                ));
            }
        }

        return new FlightResultDto(price, duration, logo, legs);
    }

    // ── Hotels ─────────────────────────────────────────────────────────────

    public async Task<List<HotelResultDto>> SearchHotelsAsync(
        string location, string checkIn, string checkOut)
    {
        var url = "https://serpapi.com/search" +
                  $"?engine=google_hotels" +
                  $"&q={Uri.EscapeDataString("Hotels in " + location)}" +
                  $"&check_in_date={checkIn}" +
                  $"&check_out_date={checkOut}" +
                  $"&currency=USD&hl=en&adults=2&api_key={_apiKey}";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return [];

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (!json.TryGetProperty("hotels_results", out var arr)) return [];

        return arr.EnumerateArray().Take(6).Select(h => new HotelResultDto(
            Name:         h.TryGetProperty("name",        out var n)   ? n.GetString()   ?? "" : "",
            Description:  h.TryGetProperty("description", out var desc)? desc.GetString()?? "" : "",
            Rating:       h.TryGetProperty("overall_rating", out var r)? r.GetDouble()      : 0,
            Reviews:      h.TryGetProperty("reviews",     out var rv)  ? rv.GetInt32()       : 0,
            PricePerNight: h.TryGetProperty("rate_per_night", out var rate)
                            && rate.TryGetProperty("lowest", out var lo) ? lo.GetString() ?? "" : "",
            HotelClass:   h.TryGetProperty("hotel_class", out var hc)  ? hc.GetString()  ?? "" : "",
            Amenities:    h.TryGetProperty("amenities",   out var am)
                            ? am.EnumerateArray().Select(a => a.GetString() ?? "").Take(5).ToList()
                            : [],
            Thumbnail:    h.TryGetProperty("thumbnail",   out var th)  ? th.GetString()  ?? "" : "",
            Link:         h.TryGetProperty("link",        out var lk)  ? lk.GetString()  ?? "" : ""
        )).ToList();
    }

    // ── Restaurants ────────────────────────────────────────────────────────

    public async Task<List<PlaceResultDto>> SearchRestaurantsAsync(string location) =>
        await SearchMapsAsync($"restaurants in {location}");

    // ── Attractions / Map ──────────────────────────────────────────────────

    public async Task<List<PlaceResultDto>> SearchAttractionsAsync(string location) =>
        await SearchMapsAsync($"top tourist attractions in {location}");

    // ── Shared Maps helper ─────────────────────────────────────────────────

    private async Task<List<PlaceResultDto>> SearchMapsAsync(string query)
    {
        var url = "https://serpapi.com/search" +
                  $"?engine=google_maps" +
                  $"&q={Uri.EscapeDataString(query)}" +
                  $"&type=search&hl=en&api_key={_apiKey}";

        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode) return [];

        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        if (!json.TryGetProperty("local_results", out var arr)) return [];

        return arr.EnumerateArray().Take(8).Select(p => new PlaceResultDto(
            Title:     p.TryGetProperty("title",   out var t) ? t.GetString()  ?? "" : "",
            Rating:    p.TryGetProperty("rating",  out var r) ? r.GetDouble()       : 0,
            Reviews:   p.TryGetProperty("reviews", out var rv)? rv.GetInt32()        : 0,
            Type:      p.TryGetProperty("type",    out var ty)? ty.GetString() ?? "" : "",
            Address:   p.TryGetProperty("address", out var a) ? a.GetString()  ?? "" : "",
            Hours:     p.TryGetProperty("hours",   out var h) ? h.GetString()  ?? "" : "",
            Phone:     p.TryGetProperty("phone",   out var ph)? ph.GetString() ?? "" : "",
            Thumbnail: p.TryGetProperty("thumbnail",out var th)? th.GetString()?? "" : ""
        )).ToList();
    }
}
