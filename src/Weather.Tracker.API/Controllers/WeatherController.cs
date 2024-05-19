using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Weather.Tracker.API.Services;

namespace Weather.Tracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json", "application/xml")]
public class WeatherController(IWeatherEntriesService service) : ControllerBase
{
    [HttpGet("entry/{id:guid}")]
    public async Task<IActionResult> GetWeatherEntry(Guid id)
    {
        var result = await service.GetWeatherEntryAsync(id);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(GetErrorMessage(result, nameof(GetWeatherEntry)));
    }

    [HttpGet("entries")]
    public async Task<IActionResult> GetWeatherEntries([FromQuery] int page, [FromQuery] int pageSize)
    {
        if (page <= 0 || pageSize <= 0)
        {
            return BadRequest("Page and pageSize parameters must be greater than zero.");
        }

        var result = await service.GetWeatherEntriesAsync(page, pageSize);
        return result.IsSuccess
            ? Ok(result.Value)
            : Problem(GetErrorMessage(result, nameof(GetWeatherEntries)));
    }

    private string GetErrorMessage(IResultBase result, string controllerAction) =>
        $"Controller action: '{controllerAction}' failed with errors: {string.Join($", {Environment.NewLine}", result.Errors)}";
}