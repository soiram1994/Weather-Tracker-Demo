using Weather.Tracker.API.Services;
using Weather.Tracker.Infrastructure;

namespace Weather.Tracker.API;

public static class ProgramExtensions
{
    private static IServiceCollection AddWeatherEntryService(this IServiceCollection services)
    {
        services.AddScoped<IWeatherEntriesService, WeatherEntriesService>();
        return services;
    }

    public static void ConfigureServices(this IServiceCollection services, string? connectionString)
    {
        services
            .AddLogging()
            .AddMapper()
            .AddWeatherEntryService()
            .AddInfrastructure(connectionString);
    }
}