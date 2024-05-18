using System.ComponentModel.DataAnnotations;

namespace Weather.Tracker.Infrastructure.Entities;

public class WeatherEntryEntity
{
    [Key] public Guid Id { get; }
}