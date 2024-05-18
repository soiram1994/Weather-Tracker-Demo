using Testcontainers.MsSql;

namespace Weather.Tracker.Tests.Fixtures;

public class SQLFixture
{
    public MsSqlContainer Container { get; } = new MsSqlBuilder()
        .WithPassword("Password123")
        .Build();

    public SQLFixture()
    {
        Container.StartAsync().Wait();
    }
}