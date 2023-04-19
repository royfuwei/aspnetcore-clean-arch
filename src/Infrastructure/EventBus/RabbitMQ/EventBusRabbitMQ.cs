using CleanArch.Domain.IntegrationEvents;
using CleanArch.Domain.IntegrationEvents.Interfaces;
using CleanArch.Infrastructure.EventBus.InMemory;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CleanArch.Infrastructure.EventBus.RabbitMQ;
public class EventBusRabbitMQ : IEventBus, IDisposable
{
    const string BROKER_NAME = "clean_arch_event_bus";

    private readonly IRabbitMQPersistentConnection _persistentConnection;

    private readonly ILogger<EventBusRabbitMQ> _logger;

    private readonly IEventBusSubscriptionsManager _subsManager;

    private readonly IServiceProvider _serviceProvider;

    private readonly int _retryCount;

    private IModel _consumerChannel;

    private string _queueName;

    public EventBusRabbitMQ(
        IRabbitMQPersistentConnection persistentConnection,
        ILogger<EventBusRabbitMQ> logger,
        IServiceProvider serviceProvider,
        IEventBusSubscriptionsManager subsManager,
        string queueName = null,
        int retryCount = 5
    )
    {
        _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _queueName = queueName;
        _serviceProvider = serviceProvider;
        _retryCount = retryCount;
        _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
        // _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        // _consumerChannel = CreateConsumerChannel();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public void Publish(IntegrationEvent @event)
    {
        throw new NotImplementedException();
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }

    public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        throw new NotImplementedException();
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        throw new NotImplementedException();
    }

    public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        throw new NotImplementedException();
    }
}