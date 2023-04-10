namespace CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;

public interface IWeatherForecastRepository : IRepository<WeatherForecast>
{
    Task<WeatherForecast> Add(WeatherForecast item);

    void Update(WeatherForecast item);

    Task<WeatherForecast> GetAsync(int id);
    
    Task<IEnumerable<WeatherForecast>> GetAllAsync();

}