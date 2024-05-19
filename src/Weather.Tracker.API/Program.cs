using Microsoft.AspNetCore.Mvc;
using Weather.Tracker.API.Services;
using Weather.Tracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("WeatherTrackerDB"));
builder.Services.AddWeatherEntryService();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddMapper();
var app = builder.Build();
app.MapGet("api/weather/entry/{id:guid}", async (IWeatherEntriesService service, [FromRoute] Guid id) =>
{
    var result = await service.GetWeatherEntryAsync(id);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(string.Join(",", result.Errors));
}).WithName("GetWeatherEntry");
app.MapGet("api/weather/entries",
    async ([FromServices] IWeatherEntriesService service, [FromQuery] int page, [FromQuery] int pageSize) =>
    {
        var result = await service.GetWeatherEntriesAsync(page, pageSize);
        return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(string.Join(",", result.Errors));
    }).WithName("GetWeatherEntries");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
await app.RunAsync();

public partial class Program
{
}

public static class ProgramExtensions
{
    public static IServiceCollection AddWeatherEntryService(this IServiceCollection services)
    {
        services.AddScoped<IWeatherEntriesService, WeatherEntriesService>();
        return services;
    }
}