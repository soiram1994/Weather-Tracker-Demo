using System;
using System.ComponentModel.DataAnnotations;

namespace Weather.Tracker.Infrastructure.Entities
{
    public class WeatherEntry
    {
        public WeatherEntry(Guid id, string description, string city, string country, double temperature, int humidity,
            int windDegree, double windSpeed, long timestamp)
        {
            Id = id;
            Description = description;
            City = city;
            Country = country;
            Temperature = temperature;
            Humidity = humidity;
            WindDegree = windDegree;
            WindSpeed = windSpeed;
            Timestamp = timestamp;
        }

        internal WeatherEntry()
        {
        }

        [Key] public Guid Id { get; private set; }

        [Required] [MaxLength(500)] public string Description { get; private set; }

        [Required] [MaxLength(100)] public string City { get; private set; }

        [Required] [MaxLength(100)] public string Country { get; private set; }

        [Required] public double Temperature { get; private set; }

        [Required] public int Humidity { get; private set; }

        [Required] public int WindDegree { get; private set; }

        [Required] public double WindSpeed { get; private set; }

        [Required] public long Timestamp { get; private set; }
    }

    public class WeatherEntryResultWithCount
    {
        public int Count { get; set; }
        public IEnumerable<WeatherEntry> WeatherEntries { get; set; }
    }
}