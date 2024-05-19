using FluentAssertions;

namespace Weather.Tracker.Tests.Integration;

public class OpenWeatherClientTests
{
    [Fact(DisplayName = "Can retrieve data for city")]
    public async Task CanRetrieveDataForCity()
    {
        // Arrange
        var client = Builder.CreateOpenWeatherMapClient(client =>
        {
            client.BaseAddress = new Uri($"https://api.openweathermap.org/data/2.5/");
        });
        var city = "London";

        // Act
        var result = await client.GetWeatherByCityName(city);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}