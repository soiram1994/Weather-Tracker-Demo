using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Weather.Tracker.Infrastructure.Persistence;

public class ApplicationDbMigrationService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IServiceProvider _serviceProvider;

    public ApplicationDbMigrationService(
        ILogger<ApplicationDbMigrationService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var scope = _serviceProvider.CreateScope();

        try
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "{message}", ex.Message);
        }
        finally
        {
            scope?.Dispose();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}