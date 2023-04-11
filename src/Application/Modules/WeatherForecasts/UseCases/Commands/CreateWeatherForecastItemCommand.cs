namespace CleanArch.Application.Modules.WeatherForecasts.UseCases.Commands;

public record CreateWeatherForecastItemCommand : IRequest<bool>
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }
}