using Weather.Tracker.Infrastructure;
using Weather.Tracker.SyncWorker;
using Weather.Tracker.SyncWorker.Services;

var builder = Host.CreateApplicationBuilder(args);
Console.WriteLine(builder.Configuration.GetConnectionString("WeatherTrackerDB"));
builder.Services
    .AddInfrastructure(builder.Configuration.GetConnectionString("WeatherTrackerDB"))
    .AddWorkerService()
    .AddOpenWeatherClient().AddLogging()
    .AddHostedService<Worker>();
Helper.SetOpenWeatherMapApiKey();
var host = builder.Build();
host.Run();

public static class ProgramExtensions
{
    public static IServiceCollection AddWorkerService(this IServiceCollection services)
    {
        services.AddScoped<IOpenWeatherRetrievalService, OpenWeatherRetrievalService>();
        return services;
    }
}