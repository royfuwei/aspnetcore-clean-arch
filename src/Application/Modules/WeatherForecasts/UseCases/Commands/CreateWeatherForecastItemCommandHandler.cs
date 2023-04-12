using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;

namespace CleanArch.Application.Modules.WeatherForecasts.UseCases.Commands;
public class CreateWeatherForecastItemCommandHandler : IRequestHandler<CreateWeatherForecastItemCommand, bool>
{
    private readonly IWeatherForecastRepository _repository;
    private readonly IMediator _mediator;

    public CreateWeatherForecastItemCommandHandler(
        IWeatherForecastRepository repository,
        IMediator meditor
    )
        => (_repository, _mediator) = (repository, meditor);
    
    public async Task<bool> Handle(CreateWeatherForecastItemCommand command, CancellationToken cancellationToken)
    {
        var entity = new WeatherForecast{
            Summary = command.Summary,
            Date = command.Date,
            TemperatureC = command.TemperatureC,
        };

        await _repository.Add(entity);
        entity.AddWeatherForcastFinishDomainEvent();

        return await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);;
    }
}