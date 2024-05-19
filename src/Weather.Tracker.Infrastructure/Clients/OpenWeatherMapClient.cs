using FluentResults;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Xml.Serialization;
using Weather.Tracker.Infrastructure.Models;

namespace Weather.Tracker.Infrastructure.Clients
{
    public class OpenWeatherMapClient(HttpClient httpClient)
    {
        private readonly string _apiKey = Environment.GetEnvironmentVariable("OPENWEATHERMAP_API_KEY")
                                          ?? throw new InvalidOperationException(
                                              "API key not found in environment variables.");

        public async Task<Result<OpenMapWeatherResponse>> GetWeatherByCityName(string cityName, string format = "json")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"weather?q={cityName}&appid={_apiKey}");

            request.Headers.Accept.Add(format == "xml"
                ? new MediaTypeWithQualityHeaderValue("application/xml")
                : new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return Result.Fail<OpenMapWeatherResponse>($"Failed to get weather data: {errorContent}");
            }

            var contentType = response.Content.Headers.ContentType?.MediaType;

            return await Deserialize(contentType, response);
        }


        private static async Task<Result<OpenMapWeatherResponse>> Deserialize(string? contentType,
            HttpResponseMessage response)
        {
            if (contentType == "application/xml")
            {
                var serializer = new XmlSerializer(typeof(OpenMapWeatherResponse));
                await using var contentStream = await response.Content.ReadAsStreamAsync();
                var weatherXML = (OpenMapWeatherResponse)serializer.Deserialize(contentStream)!;
                return Result.Ok(weatherXML);
            }

            var content = await response.Content.ReadAsStringAsync();
            var weatherJson = JsonConvert.DeserializeObject<OpenMapWeatherResponse>(content);
            return Result.Ok(weatherJson)!;
        }
    }
}