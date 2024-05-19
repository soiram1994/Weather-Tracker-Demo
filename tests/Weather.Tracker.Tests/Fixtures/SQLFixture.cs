using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Weather.Tracker.Infrastructure.Persistence;

namespace Weather.Tracker.Tests.Fixtures;

public class SQLFixture : IAsyncLifetime
{
    public MsSqlContainer Container { get; private set; }
    public ApplicationDbContext Context { get; private set; }

    public async Task InitializeAsync()
    {
        Container = new MsSqlBuilder()
            .WithPassword("Password123")
            .Build();

        await Container.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(Container.GetConnectionString())
            .Options;
        ApplicationDbContext context = new(options);
        Context = context;
        await context.Database.MigrateAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}