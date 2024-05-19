using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Weather.Tracker.Infrastructure;
using Weather.Tracker.Infrastructure.Clients;
using Weather.Tracker.Infrastructure.Persistence;
using Weather.Tracker.Infrastructure.Profiles;
using Weather.Tracker.Infrastructure.Repos;
using Weather.Tracker.SyncWorker.Services;

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

    public static IWeatherEntryRepo CreateWeatherEntryRepo(ApplicationDbContext context)
    {
        return new WeatherEntryRepo(context);
    }

    public static IMapper CreateMapper()
    {
        return new MapperConfiguration(cfg => { cfg.AddProfile<WeatherEntityProfile>(); }).CreateMapper();
    }

    public static IOpenWeatherRetrievalService BuildClientWorkerService(ApplicationDbContext context)
    {
        var openWeatherMapClient =
            CreateOpenWeatherMapClient(cfg => cfg.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/"));
        var weatherEntryRepo = CreateWeatherEntryRepo(context);
        var mapper = CreateMapper();
        var logger = new Logger<OpenWeatherRetrievalService>(new NullLoggerFactory());
        return new OpenWeatherRetrievalService(openWeatherMapClient, weatherEntryRepo, mapper, logger);
    }
}