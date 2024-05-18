using FluentResults;
using Newtonsoft.Json;
using Weather.Tracker.Infrastructure.Models;

namespace Weather.Tracker.Infrastructure.Clients;

public class OpenWeatherMapClient(HttpClient httpClient)
{
    private readonly string _apiKey = Environment.GetEnvironmentVariable("OPENWEATHERMAP_API_KEY")!;

    public async Task<Result<OpenMapWeatherResponse>> GetWeatherByCityName(string cityName)
    {
        var response =
            await httpClient.GetAsync(
                $"weather?q={cityName}&appid={_apiKey}");
        if (!response.IsSuccessStatusCode)
        {
            return Result.Fail<OpenMapWeatherResponse>("Failed to get weather data")!;
        }

        var content = await response.Content.ReadAsStringAsync();
        var weather = JsonConvert.DeserializeObject<OpenMapWeatherResponse>(content);
        return Result.Ok(weather)!;
    }
}