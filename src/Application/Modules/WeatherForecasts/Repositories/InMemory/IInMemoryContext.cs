using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;

namespace CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
public interface IInMemoryContext : IUnitOfWork
{
    IEnumerable<WeatherForecast> WeatherForecasts { get; set; }

}