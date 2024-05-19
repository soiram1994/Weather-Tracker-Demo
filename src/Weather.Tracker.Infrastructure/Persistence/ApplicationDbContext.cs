using Microsoft.EntityFrameworkCore;
using Weather.Tracker.Infrastructure.Entities;

namespace Weather.Tracker.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<WeatherEntry> WeatherEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WeatherEntryEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}