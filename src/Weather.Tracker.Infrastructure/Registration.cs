using System.Net.Http.Headers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Weather.Tracker.Infrastructure.Clients;
using Weather.Tracker.Infrastructure.Persistence;
using Weather.Tracker.Infrastructure.Profiles;
using Weather.Tracker.Infrastructure.Repos;

namespace Weather.Tracker.Infrastructure;

public static class Registration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString)
    {
        services.AddApplicationDbContext(connectionString);
        services.AddScoped<IWeatherEntryRepo, WeatherEntryRepo>();
        services.AddMapper();
        return services;
    }


    public static IServiceCollection AddOpenWeatherClient(this IServiceCollection services)
    {
        services.AddHttpClient<OpenWeatherMapClient>(client =>
        {
            client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddSingleton(
            new MapperConfiguration(cfg => { cfg.AddProfile<WeatherEntityProfile>(); }).CreateMapper());
        return services;
    }

    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services,
        string? connectionString)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString,
                optionsBuilder =>
                {
                    optionsBuilder
                        .EnableRetryOnFailure(10, TimeSpan.FromSeconds(5), null)
                        .UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
                });
        });
        return services;
    }
}