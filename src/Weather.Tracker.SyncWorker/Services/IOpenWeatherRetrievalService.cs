using FluentResults;

namespace Weather.Tracker.SyncWorker.Services;

public interface IOpenWeatherRetrievalService
{
    Task<Result> ExecuteAsync(string cityName = "London");
}