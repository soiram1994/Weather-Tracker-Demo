var builder = DistributedApplication.CreateBuilder(args);
var db = builder.AddSqlServer("WeatherTrackerServer").AddDatabase("WeatherTrackerDB");
builder.AddProject<Projects.Weather_Tracker_API>("API")
    .WithReference(db);
builder.AddProject<Projects.Weather_Tracker_SyncWorker>("SyncWorker")
    .WithReference(db);
builder.Build().Run();