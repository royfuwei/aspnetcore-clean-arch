namespace CleanArch.Application.Modules.WeatherForecasts.UseCases.ViewModels;

/// <summary>
/// 用ViewModel DTO 方式資料回給前端
/// 之後要用 AutoMapper 將 Entity, Aggregate 轉換 DTO
/// </summary>
public class WeatherForecastDTO
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public string? Summary { get; set; }
}
