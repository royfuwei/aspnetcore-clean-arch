using CleanArch.Domain.DomainEvents;

namespace CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;

public class WeatherForecast : EntityBase, IAggregateRoot
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }

    public WeatherForecast() {}
    public WeatherForecast(
        int id,
        DateTime date,
        int temperatureC,
        string summary
    ) => (Id, Date, TemperatureC, Summary) = (id, date,temperatureC, summary);


    /// <summary>
    /// 測試 Domain Events
    /// </summary>
    public void AddWeatherForcastFinishDomainEvent() {
        this.AddDomainEvent(new WeatherForecastCreatedEvent());
    }
}