using FluentResults;
using Weather.Tracker.Infrastructure.Models;

namespace Weather.Tracker.API.Services;

public interface IWeatherEntriesService
{
    Task<Result<PagedResult<WeatherEntryDTO>>> GetWeatherEntriesAsync(int page, int pageSize);
    Task<Result<WeatherEntryDTO>> GetWeatherEntryAsync(Guid id);
}