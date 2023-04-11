namespace CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;

public class WeatherForecast : EntityBase, IAggregateRoot
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    public WeatherForecast() {}
    public WeatherForecast(
        int id,
        DateOnly date,
        int temperatureC,
        string summary
    ) => (Id, Date, TemperatureC, Summary) = (id, date,temperatureC, summary);
}