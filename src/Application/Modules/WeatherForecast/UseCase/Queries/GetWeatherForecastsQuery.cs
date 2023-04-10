namespace CleanArch.Application.Modules.WeatherForecast.UseCase.Queries;

public record GetWeatherForecastsQuery : IRequest<IEnumerable<WeatherForecastAggregate>>;