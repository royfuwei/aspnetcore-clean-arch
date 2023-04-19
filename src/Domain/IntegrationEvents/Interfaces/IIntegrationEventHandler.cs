namespace CleanArch.Domain.IntegrationEvents.Interfaces;

/// <summary>
/// 建立處理IntegrationEvent 的Handler 介面
/// </summary>
/// <typeparam name="TIntegrationEvent"></typeparam>
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// 非同步處理IntegrationEvent
    /// </summary>
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationEventHandler
{
}