using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using CleanArch.Domain.SeedWork.Interfaces;

namespace CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
public class InMemoryWeatherForecastRepository : IWeatherForecastRepository
{
    protected IEnumerable<WeatherForecast> _dataMap = new List<WeatherForecast>() {};
    protected ILogger<InMemoryWeatherForecastRepository> _logger;

    IUnitOfWork IRepository<WeatherForecast>.UnitOfWork => throw new NotImplementedException();

    public InMemoryWeatherForecastRepository(
        ILogger<InMemoryWeatherForecastRepository> logger
    )
    {
        _logger = logger;
        InitialData();
    }

    private readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private void InitialData() {
        int[] temperatureCList = new[] { 7, 17, 27, 6, 37 };
        string[] summaryList = new[] { "Chilly", "Mild", "Warm", "Cool", "Hot" };

        foreach (var (_, index) in summaryList.Select((_, index) => (_, index)))
        {
            int id = index+1 ;
            _dataMap = _dataMap.Append(
                new WeatherForecast(
                    id: id,
                    date: DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    temperatureC: temperatureCList[index],
                    summary: summaryList[index]
                )
            );
        }

        _logger.LogInformation("Run InitialData() _dataMap count: {Count}", _dataMap.Count());
    }

    public Task<WeatherForecast> Add(WeatherForecast item)
    {
        item.Id = _dataMap.Count() + 1;
        _dataMap = _dataMap.Append(item);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<WeatherForecast>> GetAllAsync() => Task.FromResult(_dataMap);

    public Task<WeatherForecast> GetAsync(int id) 
        => Task.FromResult(_dataMap.Where(item => item.Id == id).FirstOrDefault()!);

    public void Update(WeatherForecast item)
    {
        var dataList = _dataMap.ToList();
        var index = dataList.IndexOf(item);
        dataList[index] = item;
        _dataMap = dataList;
    }
}