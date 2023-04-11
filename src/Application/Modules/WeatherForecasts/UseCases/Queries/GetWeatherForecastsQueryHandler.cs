using CleanArch.Application.Modules.WeatherForecasts.UseCases.ViewModels;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;

namespace CleanArch.Application.Modules.WeatherForecasts.UseCases.Queries;

public class GetWeatherForecastsQueryHandler 
    : IRequestHandler<GetWeatherForecastsQuery, IEnumerable<WeatherForecastDTO>>
{
    private readonly IWeatherForecastRepository _weatherForecastRepository;

    public GetWeatherForecastsQueryHandler(IWeatherForecastRepository weatherForecastRepository)
        => (_weatherForecastRepository) = (weatherForecastRepository);
    

    /// <summary>
    /// async Handle()
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Task.FromResult(IEnumerable<WeatherForecastDTO>)</returns>
    public async Task<IEnumerable<WeatherForecastDTO>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
    {
        var data = await _weatherForecastRepository.GetAllAsync();
        
        return data.Select(item => new WeatherForecastDTO
        {
            Id = item.Id,
            Date = item.Date,
            TemperatureC = item.TemperatureC,
            Summary = item.Summary
        });
    }
}