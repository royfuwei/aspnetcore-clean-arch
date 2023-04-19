namespace CleanArch.Domain.IntegrationEvents.Interfaces;
public interface IEventBus
{
    /// <summary>
    /// 發布 event (messageQueue: RabbitMQ)
    /// </summary>
    void Publish(IntegrationEvent @event);

    /// <summary>
    /// 訂閱訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    /// <returns></returns>
    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    void Unsubscribe<T, TH>()
        where TH : IIntegrationEventHandler<T>
        where T : IntegrationEvent;

    void SubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    void UnsubscribeDynamic<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;
}