using CleanArch.Domain.IntegrationEvents;
using CleanArch.Domain.IntegrationEvents.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanArch.Infrastructure.EventBus.InMemory;
public class EventBusInMemory : IEventBus, IDisposable
{

    private readonly ILogger<EventBusInMemory> _logger;

    private readonly InMemoryEventBusSubscriptionsManager _subsManager;

    private readonly IServiceProvider _serviceProvider;




    public EventBusInMemory(
        ILogger<EventBusInMemory> logger,
        IServiceProvider serviceProvider,
        InMemoryEventBusSubscriptionsManager subsManager
    )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider;
        _subsManager = subsManager;
    }

    public void Dispose()
    {
        _subsManager.Clear();
    }


    public void Publish(IntegrationEvent @event)
    {
        _logger.LogInformation("Run InMemory Publish @event {Event}", @event);
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();
        _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH));
        _subsManager.AddSubscription<T, TH>();
    }

    public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH));
        _subsManager.AddDynamicSubscription<TH>(eventName);
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = _subsManager.GetEventKey<T>();

        _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

        _subsManager.RemoveSubscription<T, TH>();
    }

    public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        _subsManager.RemoveDynamicSubscription<TH>(eventName);
    }
}