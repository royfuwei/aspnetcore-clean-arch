using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using CleanArch.Domain.DomainEvents;

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
            Date = DateOnly.FromDateTime(command.Date),
            TemperatureC = command.TemperatureC,
        };

        await _repository.Add(entity);

        await CreatedWeatherForecastItemDomainEvent(entity, cancellationToken);
        return true;
    }

    private async Task CreatedWeatherForecastItemDomainEvent(WeatherForecast entity, CancellationToken cancellationToken)
    {
        /* entity.AddDomainEvent(new WeatherForecastCreatedEvent());
        _repository.UnitOfWork.SaveChangesAsync(cancellationToken); */
        await _mediator.Publish(new WeatherForecastCreatedEvent());
        return;
    }
}