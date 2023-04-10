using CleanArch.Application.Modules.WeatherForecast.UseCases.Queries;
using CleanArch.Application.Modules.WeatherForecast.UseCases.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/weather-forecast")]
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

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecastDTO>> Get()
    {
        return await _mediator.Send(new GetWeatherForecastsQuery());
    }
}
