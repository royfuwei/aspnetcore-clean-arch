using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using CleanArch.Domain.SeedWork.Interfaces;

namespace CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
public class InMemoryWeatherForecastRepository : IWeatherForecastRepository
{
    protected IEnumerable<WeatherForecast> _dataMap = new List<WeatherForecast>() {};

    IUnitOfWork IRepository<WeatherForecast>.UnitOfWork => throw new NotImplementedException();

    public InMemoryWeatherForecastRepository() => InitialData();

    private readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private void InitialData() {
        Console.WriteLine($"InitialData !! _dataMap.Count(): {_dataMap.Count()}");
        if (_dataMap.Count() > 0) return;
        var rng = new Random();

        var data = Enumerable.Range(1, 5).Select(index => {

            var date = DateOnly.FromDateTime(DateTime.Now.AddDays(index));
            var temperatureC = rng.Next(-20, 55);
            var summary = Summaries[rng.Next(Summaries.Length)];
            return new WeatherForecast
            {
                Date = date,
                TemperatureC = temperatureC,
                Summary = summary
            };
        });

        _dataMap = _dataMap.Concat(data);
    }

    public Task<WeatherForecast> Add(WeatherForecast item)
    {
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