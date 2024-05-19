using FluentResults;
using Microsoft.EntityFrameworkCore;
using Weather.Tracker.Infrastructure.Entities;
using Weather.Tracker.Infrastructure.Models;
using Weather.Tracker.Infrastructure.Persistence;

namespace Weather.Tracker.Infrastructure.Repos;

public class WeatherEntryRepo(ApplicationDbContext context) : IWeatherEntryRepo
{
    public async Task<Result<WeatherEntry>> GetWeatherEntryAsync(Guid id)
    {
        try
        {
            var entry = await context.WeatherEntries.FirstOrDefaultAsync(x => x.Id == id);
            return entry == null ? Result.Fail<WeatherEntry>("Weather entry not found") : Result.Ok(entry);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Fail<WeatherEntry>(e.Message);
        }
    }

    public async Task<Result> AddWeatherEntryAsync(WeatherEntry weatherEntry)
    {
        try
        {
            await context.WeatherEntries.AddAsync(weatherEntry);
            var modCount = await context.SaveChangesAsync();
            if (modCount == 0)
            {
                return Result.Fail("Failed to save weather data");
            }

            return Result.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Fail(e.Message);
        }
    }

    public async Task<Result<WeatherEntryResultWithCount>> GetPagedWeatherAsync(int page,
        int pageSize)
    {
        try
        {
            var entries = await context.WeatherEntries
                .OrderByDescending(x => x.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var totalCount = await context.WeatherEntries.CountAsync();
            return Result.Ok((new WeatherEntryResultWithCount
            {
                Count = totalCount,
                WeatherEntries = entries
            }));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Fail<WeatherEntryResultWithCount>(e.Message);
        }
    }
}

public interface IWeatherEntryRepo
{
    public Task<Result<WeatherEntry>> GetWeatherEntryAsync(Guid id);
    public Task<Result> AddWeatherEntryAsync(WeatherEntry weatherEntry);
    public Task<Result<WeatherEntryResultWithCount>> GetPagedWeatherAsync(int page, int pageSize);
}