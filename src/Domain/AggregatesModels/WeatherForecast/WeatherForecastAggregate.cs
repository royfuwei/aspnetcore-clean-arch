namespace CleanArch.Domain.AggregatesModels.WeatherForecast;

public class WeatherForecastAggregate : EntityBase, IAggregateRoot
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    public WeatherForecastAggregate() {}
    public WeatherForecastAggregate(
        int id,
        DateOnly date,
        int temperatureC,
        string summary
    ) => (Id, Date, TemperatureC, Summary) = (id, date,temperatureC, summary);
}