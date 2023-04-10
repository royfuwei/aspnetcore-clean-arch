using CleanArch.Application.Modules.WeatherForecast.UseCase.ViewModels;
namespace CleanArch.Application.Modules.WeatherForecast.UseCase.Queries;

public class GetWeatherForecastsQueryHandler 
    : IRequestHandler<GetWeatherForecastsQuery, IEnumerable<WeatherForecastDTO>>
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    

    /// <summary>
    /// async Handle()
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Task.FromResult(IEnumerable<WeatherForecastDTO>)</returns>
    public Task<IEnumerable<WeatherForecastDTO>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
    {
        var rng = new Random();
        
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecastDTO
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        }));
    }
}