var builder = DistributedApplication.CreateBuilder(args);
var db = builder.AddSqlServer("WeatherTrackerServer").WithHealthCheck()
    .AddDatabase("WeatherTrackerDB");
var api = builder.AddProject<Projects.Weather_Tracker_API>("API")
    .WithReference(db)
    .WaitFor(db);
builder.AddProject<Projects.Weather_Tracker_SyncWorker>("SyncWorker")
    .WithReference(db)
    .WaitFor(db)
    .WaitFor(api);
builder.Build().Run();