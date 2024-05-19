using Weather.Tracker.SyncWorker.Services;

namespace Weather.Tracker.SyncWorker;

public class Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    : BackgroundService
{
    private readonly int _delay = int.Parse(Environment.GetEnvironmentVariable("DELAY") ?? "1");

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
        logger.LogInformation("Delay set to: {Delay} hours", _delay);
        using var scope = serviceProvider.CreateScope();
        var openWeatherRetrievalService = scope.ServiceProvider.GetRequiredService<IOpenWeatherRetrievalService>();
        while (stoppingToken.IsCancellationRequested == false)
        {
            await openWeatherRetrievalService.ExecuteAsync();
            await Task.Delay(TimeSpan.FromHours(_delay), stoppingToken);
        }
    }
}