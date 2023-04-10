using CleanArch.Application.Modules.WeatherForecast.UseCase.ViewModels;
namespace CleanArch.Application.Modules.WeatherForecast.UseCase.Queries;

public record GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecastDTO>>;