namespace Weather.Tracker.Infrastructure.Models;

public class WeatherEntryDTO
{
    public string Description { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public double Temperature { get; set; }
    public string Humidity { get; set; }
    public string WindDegree { get; set; }
    public string WindSpeed { get; set; }
    public long Timestamp { get; set; }
}