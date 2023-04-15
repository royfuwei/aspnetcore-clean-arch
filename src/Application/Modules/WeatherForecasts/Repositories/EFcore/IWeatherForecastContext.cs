using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;


namespace CleanArch.Application.Modules.WeatherForecasts.Repositories.EFcore;
public interface IWeatherForecastContext : IUnitOfWork
{
    DbSet<WeatherForecast> WeatherForecasts { get; }
}