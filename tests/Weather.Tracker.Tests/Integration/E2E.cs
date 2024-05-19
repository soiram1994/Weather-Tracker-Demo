using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Weather.Tracker.Infrastructure.Entities;
using Weather.Tracker.Tests.Fixtures;

namespace Weather.Tracker.Tests.Integration;

public class E2E : IClassFixture<SQLFixture>, IClassFixture<WebApplicationFactory<Program>>
{
    private readonly SQLFixture _fixture;
    private readonly WebApplicationFactory<Program> _webApplication;

    public E2E(SQLFixture fixture, WebApplicationFactory<Program> webApplication)
    {
        _fixture = fixture;
        _webApplication = webApplication;
    }

    [Fact(DisplayName = "Weather data is retrieved, stored and queried")]
    public async Task WeatherDataIsRetrievedStoredAndQueried()
    {
        // Arrange
        var workerService = Builder.BuildClientWorkerService(_fixture.Context);
        var apiClient = _webApplication.WithWebHostBuilder(b =>
        {
            b.UseSetting("ConnectionStrings:WeatherTrackerDB",
                _fixture.Container.GetConnectionString());
        }).CreateClient();

        // Act
        var result = await workerService.ExecuteAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Act
        var response = await apiClient.GetAsync("api/weather/entries?page=1&pageSize=1");


        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status code {response.StatusCode}: {content}");
        }
    }

    [Fact(DisplayName = "Weather data is stored and retrieved")]
    public async Task WeatherDataIsStoredAndRetrieved()
    {
        // Arrange
        var workerService = Builder.BuildClientWorkerService(_fixture.Context);
        var apiClient = _webApplication.WithWebHostBuilder(b =>
        {
            b.UseSetting("ConnectionStrings:WeatherTrackerDB",
                _fixture.Container.GetConnectionString());
        }).CreateClient();

        // Act
        var result = await workerService.ExecuteAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Arrange
        var entry = await _fixture.Context.WeatherEntries.FirstOrDefaultAsync();

        // Assert
        entry.Should().NotBeNull();

        // Act
        var response = await apiClient.GetAsync($"api/weather/entry/{entry.Id}");

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Request failed with status code {response.StatusCode}: {content}");
        }
    }
}