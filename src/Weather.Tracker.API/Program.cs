using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Weather.Tracker.API;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddLogging()
    .ConfigureServices(builder.Configuration.GetConnectionString("WeatherTrackerDB"));
builder.Services.AddControllers();
builder.Services.AddHostedService<ApplicationDbMigrationService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
await app.RunAsync();

public partial class Program
{
}