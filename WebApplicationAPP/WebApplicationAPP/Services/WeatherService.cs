using Microsoft.Extensions.Caching.Memory;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly IMemoryCache _cache;

    public WeatherService(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _config = config;
        _cache = cache;
    }

    public async Task<object> GetWeatherAsync()
    {
        var cacheKey = "weather_sanjose";

        if (_cache.TryGetValue(cacheKey, out object cached))
            return cached;

        var apiKey = _config["Weather:ApiKey"];
        var city = _config["Weather:City"] ?? "San Jose,CR";

        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Error obteniendo clima");

        var json = await response.Content.ReadAsStringAsync();

        using var doc = System.Text.Json.JsonDocument.Parse(json);
        var root = doc.RootElement;

        var rawDesc = root.GetProperty("weather")[0].GetProperty("description").GetString();
        string desc = rawDesc.ToLower();

        var result = new
        {
            temp = root.GetProperty("main").GetProperty("temp").GetDouble(),
            city = root.GetProperty("name").GetString(),
            desc = desc
        };

        var minutes = int.Parse(_config["Weather:CacheMinutes"] ?? "10");
        _cache.Set(cacheKey, result, TimeSpan.FromMinutes(minutes));

        return result;
    }
}