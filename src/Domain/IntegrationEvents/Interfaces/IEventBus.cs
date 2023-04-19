namespace CleanArch.Domain.IntegrationEvents.Interfaces;
public interface IEventBus
{
    /// <summary>
    /// 發布 event (messageQueue, inMemory)
    /// </summary>
    void Publish();

    /// <summary>
    /// 訂閱訊息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TH"></typeparam>
    /// <returns></returns>
    void Subscribe<T, TH>()
        where T : IIntegrationEvent
        where TH : IIntegrationEventHandler<T>;

}