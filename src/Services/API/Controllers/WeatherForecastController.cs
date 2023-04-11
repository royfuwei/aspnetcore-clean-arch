using CleanArch.Application.Modules.WeatherForecasts.UseCases.Commands;
using CleanArch.Application.Modules.WeatherForecasts.UseCases.Queries;
using CleanArch.Application.Modules.WeatherForecasts.UseCases.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/weather-forecasts")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(
        IMediator mediator,
        ILogger<WeatherForecastController> logger
    )
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// GetWeatherForecast List
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecastDTO>> Get()
    {
        return await _mediator.Send(new GetWeatherForecastsQuery());
    }


    [HttpPost(Name = "AddWeatherForecast")]
    public async Task<bool> Add(
        [FromBody] CreateWeatherForecastItemCommand command
    )
    {
        return await _mediator.Send(command);
    }
}
