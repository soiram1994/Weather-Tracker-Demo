namespace Weather.Tracker.Infrastructure.Models;

public class WeatherEntryDTO
{
    public string Description { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public double Temperature { get; set; }
    public int Humidity { get; set; }
    public int WindDegree { get; set; }
    public int WindSpeed { get; set; }
    public long Timestamp { get; set; }
}