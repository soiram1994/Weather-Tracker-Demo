namespace Weather.Tracker.Infrastructure;

public static class Helper
{
    /// <summary>
    /// Not advised
    /// Due to this being a sample project, this method eases the use of the OpenWeatherMap API key
    /// </summary>
    public static void SetOpenWeatherMapApiKey()
    {
        Environment.SetEnvironmentVariable("OPENWEATHERMAP_API_KEY", "ede4feae499af9d350f53d3e2edf4691");
    }
}