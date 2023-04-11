using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using CleanArch.Domain.DomainEvents;

namespace CleanArch.Application.Modules.WeatherForecasts.EventHandlers.DomainEventHandlers;
public class WeatherForecastCreatedEventHandler : INotificationHandler<WeatherForecastCreatedEvent>
{
    private readonly IWeatherForecastRepository _repository;
    private readonly ILogger<WeatherForecastCreatedEventHandler> _logger;
    public WeatherForecastCreatedEventHandler(
        ILogger<WeatherForecastCreatedEventHandler> logger,
        IWeatherForecastRepository repository
    ) => (_logger, _repository) = (logger, repository);

    public async Task Handle(WeatherForecastCreatedEvent notification, CancellationToken cancellationToken)
    {
        var data = await _repository.GetAllAsync();
        _logger.LogInformation("DomainEvent: {DomainEvent}, data count: {Count}", notification.GetType(), data.Count());
    }
}