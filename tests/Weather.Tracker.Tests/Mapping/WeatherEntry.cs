using AutoMapper;
using FluentAssertions;
using Weather.Tracker.Infrastructure.Entities;
using Weather.Tracker.Infrastructure.Models;
using Weather.Tracker.Infrastructure.Profiles;

namespace Weather.Tracker.Tests.Mapping;

public class WeatherEntryMapping
{
    [Fact(DisplayName = "Weather response maps to WeatherEntry")]
    public void WeatherResponseMapsToWeatherEntry()
    {
        // Arrange
        var weatherResponse = new OpenMapWeatherResponse()
        {
            Name = "London",
            Sys = new Sys() { Country = "GB" },
            Main = new Main() { Temp = 10.0, Humidity = 50 },
            Wind = new Wind() { Deg = 180, Speed = 10 },
            Dt = 1633663200,
            Weather =
            [
                new OpenMapWeather
                {
                    Description = "Cloudy",
                }
            ]
        };
        var mapper = new MapperConfiguration(cfg => cfg.AddProfile<WeatherEntityProfile>()).CreateMapper();

        // Act
        var weatherEntry = mapper.Map<WeatherEntry>(weatherResponse);

        // Assert
        weatherEntry.Should().NotBeNull();
        weatherEntry.City.Should().Be("London");
        weatherEntry.Country.Should().Be("GB");
        weatherEntry.Temperature.Should().Be(10.0);
        weatherEntry.Humidity.Should().Be(50);
        weatherEntry.WindDegree.Should().Be(180);
        weatherEntry.WindSpeed.Should().Be(10);
        weatherEntry.Description.Should().Be("Cloudy");
    }
}