using FluentAssertions;
using Weather.Tracker.Infrastructure.Entities;
using Weather.Tracker.Infrastructure.Repos;
using Weather.Tracker.Tests.Fixtures;

namespace Weather.Tracker.Tests.Integration;

public class RepositoryIntegrationTests(SQLFixture fixture) : IClassFixture<SQLFixture>
{
    private readonly IWeatherEntryRepo _repo = Builder.CreateWeatherEntryRepo(fixture.Context);

    [Fact(DisplayName = "Can insert and retrieve data")]
    public async Task CanInsertAndRetrieveData()
    {
        // Arrange
        var entry = new WeatherEntry(Guid.NewGuid(), "cloudy", "London", "UK", 10.0, 50, 90, 10,
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

        // Act
        var addedResult = await _repo.AddWeatherEntryAsync(entry);

        // Assert
        addedResult.IsSuccess.Should().BeTrue();

        // Act
        var result = await _repo.GetWeatherEntryAsync(entry.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var value = result.Value;
        value.City.Should().Be(entry.City);
        value.Country.Should().Be(entry.Country);
        value.Description.Should().Be(entry.Description);
        value.Humidity.Should().Be(entry.Humidity);
        value.Temperature.Should().Be(entry.Temperature);
        value.Timestamp.Should().Be(entry.Timestamp);
        value.WindDegree.Should().Be(entry.WindDegree);
        value.WindSpeed.Should().Be(entry.WindSpeed);
    }
}