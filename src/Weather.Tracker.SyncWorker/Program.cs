using Weather.Tracker.Infrastructure;
using Weather.Tracker.SyncWorker;
using Weather.Tracker.SyncWorker.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("WeatherTrackerDB"));
builder.Services.AddWorkerService();
builder.Services.AddOpenWeatherClient();
builder.Services.AddLogging();
Helper.SetOpenWeatherMapApiKey();
builder.Services.AddHostedService<Worker>();
var host = builder.Build();
host.Run();

public static class ProgramExtensions
{
    public static void AddWorkerService(this IServiceCollection services)
    {
        services.AddScoped<IOpenWeatherRetrievalService, OpenWeatherRetrievalService>();
    }
}