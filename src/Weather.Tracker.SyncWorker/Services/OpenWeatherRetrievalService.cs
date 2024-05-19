using AutoMapper;
using FluentResults;
using Weather.Tracker.Infrastructure.Clients;
using Weather.Tracker.Infrastructure.Entities;
using Weather.Tracker.Infrastructure.Models;
using Weather.Tracker.Infrastructure.Repos;

namespace Weather.Tracker.SyncWorker.Services;

public class OpenWeatherRetrievalService(
    OpenWeatherMapClient openWeatherMapClient,
    IWeatherEntryRepo weatherEntryRepo,
    IMapper mapper,
    ILogger<OpenWeatherRetrievalService> logger) : IOpenWeatherRetrievalService
{
    public async Task<Result> ExecuteAsync(string cityName = "London")
    {
        // Get weather data from OpenWeatherMap
        var openWeatherResult = await GetWeatherByCityName(cityName);
        if (openWeatherResult.IsFailed)
        {
            return ErrorFlow(openWeatherResult);
        }

        // Map to WeatherEntry
        var mappedEntry = MapToWeatherEntry(openWeatherResult.Value);

        // Add weather data to database and complete flow
        var addWeatherResult = await weatherEntryRepo.AddWeatherEntryAsync(mappedEntry);
        return addWeatherResult.IsFailed ? ErrorFlow(addWeatherResult) : Result.Ok();
    }

    private Result ErrorFlow(IResultBase result)
    {
        logger.LogError("Error while executing OpenWeatherClientService. With messages: {Message}",
            string.Join(",", result.Errors));
        return Result.Fail(result.Errors);
    }

    private WeatherEntry MapToWeatherEntry(OpenMapWeatherResponse openMapWeatherResponse) =>
        mapper.Map<WeatherEntry>(openMapWeatherResponse);

    private async Task<Result<OpenMapWeatherResponse>> GetWeatherByCityName(string cityName)
    {
        var result = await openWeatherMapClient.GetWeatherByCityName(cityName);
        return result;
    }

    private Task<Result> AddWeatherAsync(WeatherEntry weatherEntry) =>
        weatherEntryRepo.AddWeatherEntryAsync(weatherEntry);
}