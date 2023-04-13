using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Infrastructure.Persistence.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Services.API.Infrastructure.EFCore;

/// <summary>
/// EFCore WeatherForecastContext Initialise, Seed Data
/// </summary>
public class WeatherForecastContextSeed
{
     private readonly ILogger<WeatherForecastContext> _logger;
    private readonly WeatherForecastContext _context;

    public WeatherForecastContextSeed(
        ILogger<WeatherForecastContext> logger,
        WeatherForecastContext context
    )
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// 初始化時，如果連線db 是 sql server 就migration
    /// </summary>
    /// <returns></returns>
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await CreateWeatherForecasts();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while seeding the database.");
            throw;
        }
    }


    /// <summary>
    /// 初始化 db 沒有資料，建立 WeatherForecasts data
    /// </summary>
    /// <returns></returns>
    private async Task CreateWeatherForecasts()
    {
        if (_context.WeatherForecasts.Any()) return;

        IEnumerable<WeatherForecast> weatherForecasts = new List<WeatherForecast>();

        int[] temperatureCList = new[] { 7, 17, 27, 6, 37 };
        string[] summaryList = new[] { "Chilly", "Mild", "Warm", "Cool", "Hot" };

        foreach (var (_, index) in summaryList.Select((_, index) => (_, index)))
        {
            int id = index+1 ;
            var weatherForecast = new WeatherForecast(
                id: id,
                date: DateTime.Now.AddDays(index),
                temperatureC: temperatureCList[index],
                summary: summaryList[index]
            );
            _context.Add(weatherForecast);
            await _context.SaveChangesAsync();
        }
    }
}