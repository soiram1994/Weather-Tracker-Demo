using System.Xml.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Weather.Tracker.Infrastructure.Models;
using Weather.Tracker.Tests.Fixtures;

namespace Weather.Tracker.Tests.Integration;

public class CompleteFlow(SQLFixture fixture, WebApplicationFactory<Program> webApplication)
    : IClassFixture<SQLFixture>, IClassFixture<WebApplicationFactory<Program>>
{
    [Fact(DisplayName = "Weather data is retrieved, stored and queried")]
    public async Task WeatherDataIsRetrievedStoredAndQueried()
    {
        // Arrange
        var workerService = Builder.BuildClientWorkerService(fixture.Context);
        var apiClient = webApplication.WithWebHostBuilder(b =>
        {
            b.UseSetting("ConnectionStrings:WeatherTrackerDB",
                fixture.Container.GetConnectionString());
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

        var contentString = await response.Content.ReadAsStringAsync();
        var xmlSerializer = new XmlSerializer(typeof(PagedResult<WeatherEntryDTO>));
        var weatherEntries = (PagedResult<WeatherEntryDTO>)xmlSerializer.Deserialize(new StringReader(contentString))!;
        var weatherEntryDtos = weatherEntries.Items;

        weatherEntryDtos.Should().NotBeNullOrEmpty();
        weatherEntryDtos.Should().HaveCount(1);
        weatherEntryDtos.First().City.Should().NotBeNullOrEmpty();
    }
}