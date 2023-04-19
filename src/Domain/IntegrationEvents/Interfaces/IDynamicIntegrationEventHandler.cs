namespace CleanArch.Domain.IntegrationEvents.Interfaces;

public interface IDynamicIntegrationEventHandler
{
    Task Handle(dynamic eventData);
}
