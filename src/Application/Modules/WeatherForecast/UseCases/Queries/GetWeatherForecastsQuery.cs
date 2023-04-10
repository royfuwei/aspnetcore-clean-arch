using CleanArch.Application.Modules.WeatherForecast.UseCases.ViewModels;
namespace CleanArch.Application.Modules.WeatherForecast.UseCases.Queries;

public record GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecastDTO>>;