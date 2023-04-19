using CleanArch.Domain.IntegrationEvents;
using CleanArch.Domain.IntegrationEvents.Interfaces;

namespace CleanArch.Application.Modules.WeatherForecasts.EventHandlers.IntegrationEventHandlers;
public class WeatherForecastTestIEventHandler : IIntegrationEventHandler<WeatherForecastTestIEvent>
{
    public Task Handle(WeatherForecastTestIEvent @event)
    {
        throw new NotImplementedException();
    }
}