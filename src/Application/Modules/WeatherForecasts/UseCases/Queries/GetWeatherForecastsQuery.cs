using CleanArch.Application.Modules.WeatherForecasts.UseCases.ViewModels;

namespace CleanArch.Application.Modules.WeatherForecasts.UseCases.Queries;
public record GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecastDTO>>;