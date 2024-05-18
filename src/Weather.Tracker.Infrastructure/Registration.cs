using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Weather.Tracker.Infrastructure.Clients;

namespace Weather.Tracker.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClient<OpenWeatherMapClient>(configureClient: client =>
        {
            client.BaseAddress = new Uri($"https://api.openweathermap.org/data/2.5/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        return services;
    }
}