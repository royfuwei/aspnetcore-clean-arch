using CleanArch.Domain.IntegrationEvents;
using CleanArch.Domain.IntegrationEvents.Interfaces;

namespace CleanArch.Infrastructure.EventBus.InMemory;
public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
    private readonly List<Type> _eventTypes;

    public bool IsEmpty => throw new NotImplementedException();

    public event EventHandler<string> OnEventRemoved;

    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();

        DoAddSubscription(typeof(TH), eventName, isDynamic: false);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }
    }

    
    public void AddDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        DoAddSubscription(typeof(TH), eventName, true);
    }
    
    private void DoAddSubscription(Type handlerType, string eventName, bool isDynamic)
    {
        if(!HasSubscriptionsForEvent(eventName)) _handlers.Add(eventName, new List<SubscriptionInfo>());

        if(_handlers[eventName].Any(s => s.HandlerType == handlerType))
            throw new ArgumentException(
                $"Handler Type {handlerType.Name} 已經註冊 '{eventName}'", nameof(handlerType));

        if(isDynamic) _handlers[eventName].Add(SubscriptionInfo.Dynamic(handlerType));
        _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
    }

    public void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var handlerToRemove = FindSubscriptionToRemove<T, TH>();
        var eventName = GetEventKey<T>();
        DoRemoveHandler(eventName, handlerToRemove);
    }

    public void RemoveDynamicSubscription<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        var handlerToRemove = FindDynamicSubscriptionToRemove<TH>(eventName);
        DoRemoveHandler(eventName, handlerToRemove);
    }

    private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    private SubscriptionInfo FindDynamicSubscriptionToRemove<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler
    {
        return DoFindSubscriptionToRemove(eventName, typeof(TH));
    }

    private SubscriptionInfo DoFindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName))
        {
            return null;
        }

        return _handlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);

    }

    private void DoRemoveHandler(string eventName, SubscriptionInfo subsToRemove)
    {
        if(subsToRemove != null)
        {
            _handlers[eventName].Remove(subsToRemove);
            if (!_handlers[eventName].Any())
            {
                _handlers.Remove(eventName);
                var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);
                if (eventType != null)
                {
                    _eventTypes.Remove(eventType);
                }

                RaiseOnEventRemoved(eventName);

            }
        }

    }

    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }
    

    public void Clear() => _handlers.Clear();

    public string GetEventKey<T>() => typeof(T).Name;

    public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

    public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
    {
        var key = GetEventKey<T>();
        return HasSubscriptionsForEvent(key);
    }

    /// <summary>
    /// 檢查Dictionary 有沒有此 eventName 
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

}