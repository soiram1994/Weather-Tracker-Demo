using AutoMapper;
using FluentResults;
using Weather.Tracker.Infrastructure.Entities;
using Weather.Tracker.Infrastructure.Models;
using Weather.Tracker.Infrastructure.Repos;

namespace Weather.Tracker.API.Services;

public class WeatherEntriesService(IWeatherEntryRepo repository, ILogger<WeatherEntriesService> logger, IMapper mapper)
    : IWeatherEntriesService
{
    public async Task<Result<PagedResult<WeatherEntryDTO>>> GetWeatherEntriesAsync(int page, int pageSize)
    {
        var weatherEntries = await repository.GetPagedWeatherAsync(page, pageSize);
        if (weatherEntries.IsFailed)
        {
            var message = string.Join(",", weatherEntries.Errors);
            logger.LogError("Error while retrieving weather entries. With messages: {Message}", message);
            return Result.Fail<PagedResult<WeatherEntryDTO>>(message);
        }

        var mappedEntries = weatherEntries.Value.WeatherEntries.Select(MapToWeatherEntryDto);
        return Result.Ok(new PagedResult<WeatherEntryDTO>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = weatherEntries.Value.Count,
            Items = mappedEntries.ToList()
        });
    }

    public async Task<Result<WeatherEntryDTO>> GetWeatherEntryAsync(Guid id)
    {
        var weatherEntry = await repository.GetWeatherEntryAsync(id);
        if (weatherEntry.IsFailed)
        {
            var message = string.Join(",", weatherEntry.Errors);
            logger.LogError("Error while retrieving weather entry. With messages: {Message}", message);
            return Result.Fail<WeatherEntryDTO>(message);
        }

        return Result.Ok(MapToWeatherEntryDto(weatherEntry.Value));
    }

    private WeatherEntryDTO MapToWeatherEntryDto(WeatherEntry weatherEntry) =>
        mapper.Map<WeatherEntryDTO>(weatherEntry);
}