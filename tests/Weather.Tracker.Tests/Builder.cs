using Weather.Tracker.Infrastructure;
using Weather.Tracker.Infrastructure.Clients;

namespace Weather.Tracker.Tests;

public static class Builder
{
    public static OpenWeatherMapClient CreateOpenWeatherMapClient(Action<HttpClient>? configureClient = default)
    {
        Helper.SetOpenWeatherMapApiKey();
        var client = new HttpClient();
        configureClient?.Invoke(client);
        return new OpenWeatherMapClient(client);
    }
}