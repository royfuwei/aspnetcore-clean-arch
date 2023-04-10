using CleanArch.Domain.SeedWork;
using CleanArch.Domain.SeedWork.Interfaces;

namespace CleanArch.Domain.AggregatesModels.WeatherForecast;
public class WeatherForecast : EntityBase, IAggregateRoot
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}