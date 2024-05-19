using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Weather.Tracker.Infrastructure.Entities;

namespace Weather.Tracker.Infrastructure.Persistence;

public class WeatherEntryEntityTypeConfiguration : IEntityTypeConfiguration<WeatherEntry>
{
    public void Configure(EntityTypeBuilder<WeatherEntry> builder)
    {
        builder.ToTable("weather_entries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(36);

        builder.Property(x => x.City)
            .HasMaxLength(255);

        builder.Property(x => x.Temperature)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(255);

        builder.Property(x => x.Humidity)
            .IsRequired();
        builder.Property(x => x.WindSpeed)
            .IsRequired();
        builder.Property(x => x.WindDegree)
            .IsRequired();
    }
}