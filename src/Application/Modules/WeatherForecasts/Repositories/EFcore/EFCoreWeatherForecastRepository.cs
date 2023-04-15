using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using CleanArch.Domain.SeedWork.Interfaces;

namespace CleanArch.Application.Modules.WeatherForecasts.Repositories.EFcore;
public class EFCoreWeatherForecastRepository : IWeatherForecastRepository
{
    private readonly IWeatherForecastContext _context;

    protected ILogger<EFCoreWeatherForecastRepository> _logger;

    public EFCoreWeatherForecastRepository(
        IWeatherForecastContext context,
        ILogger<EFCoreWeatherForecastRepository> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    
    public IUnitOfWork UnitOfWork => _context;

    public Task<WeatherForecast> Add(WeatherForecast item)
    {
        return Task.FromResult(_context.WeatherForecasts.Add(item).Entity);
    }

    public Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        IEnumerable<WeatherForecast> result =  _context.WeatherForecasts
            .Select(item => new WeatherForecast
                {
                    Id = item.Id,
                    TemperatureC = item.TemperatureC,
                    Date = item.Date,
                    Summary = item.Summary,
                }
            )
            .ToList();
        return Task.FromResult(result); 
    }

    public Task<WeatherForecast> GetAsync(int id)
    {
        var weatherForecast = _context.WeatherForecasts
            .Where(item => item.Id == id)
            .SingleOrDefault();
        return Task.FromResult(weatherForecast!);
    }

    public void Update(WeatherForecast item)
    {
        _context.WeatherForecasts
            .Update(item);
    }
}