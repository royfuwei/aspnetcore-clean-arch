using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using CleanArch.Domain.SeedWork.Interfaces;

namespace CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
public class InMemoryWeatherForecastRepository : IWeatherForecastRepository
{
    protected IEnumerable<WeatherForecast> _dataMap = new List<WeatherForecast>() {};

    protected IInMemoryContext _context;
    protected ILogger<InMemoryWeatherForecastRepository> _logger;

    IUnitOfWork IRepository<WeatherForecast>.UnitOfWork => _context;

    public InMemoryWeatherForecastRepository(
        ILogger<InMemoryWeatherForecastRepository> logger,
        IInMemoryContext context
    )
    {
        _logger = logger;
        _context = context;
        InitialData();
    }

    private readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private void InitialData() {
        IEnumerable<WeatherForecast> weatherForecasts = new List<WeatherForecast>();

        int[] temperatureCList = new[] { 7, 17, 27, 6, 37 };
        string[] summaryList = new[] { "Chilly", "Mild", "Warm", "Cool", "Hot" };

        foreach (var (_, index) in summaryList.Select((_, index) => (_, index)))
        {
            int id = index+1 ;
            weatherForecasts = weatherForecasts.Append(
                new WeatherForecast(
                    id: id,
                    date: DateTime.Now.AddDays(index),
                    temperatureC: temperatureCList[index],
                    summary: summaryList[index]
                )
            );
        }

        _context.WeatherForecasts = weatherForecasts;

        _logger.LogInformation("Run InitialData() _context.WeatherForecasts count: {Count}", _context.WeatherForecasts.Count());
    }

    public Task<WeatherForecast> Add(WeatherForecast item)
    {
        item.Id = _context.WeatherForecasts.Count() + 1;
        _context.WeatherForecasts = _context.WeatherForecasts.Append(item);

        var weatherForecastsDomainEvents = _context.WeatherForecasts
            .SelectMany(e => e.DomainEvents)
            .ToList();

        return Task.FromResult(item);
    }

    public Task<IEnumerable<WeatherForecast>> GetAllAsync() => Task.FromResult(_context.WeatherForecasts);

    public Task<WeatherForecast> GetAsync(int id) 
        => Task.FromResult(_context.WeatherForecasts.Where(item => item.Id == id).FirstOrDefault()!);

    public void Update(WeatherForecast item)
    {
        var dataList = _context.WeatherForecasts.ToList();
        var index = dataList.IndexOf(item);
        dataList[index] = item;
        _context.WeatherForecasts = dataList;
    }
}