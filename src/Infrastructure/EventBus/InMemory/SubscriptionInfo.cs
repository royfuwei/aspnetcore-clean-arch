namespace CleanArch.Infrastructure.EventBus.InMemory;
public class SubscriptionInfo
{
    public bool IsDynamic { get; }
    public Type HandlerType { get; }

    SubscriptionInfo(
        bool isDynamic, Type handlerType
    ) => (IsDynamic, HandlerType) = (isDynamic, handlerType);

    public static SubscriptionInfo Dynamic(Type handlerType) =>
        new SubscriptionInfo(true, handlerType);

    public static SubscriptionInfo Typed(Type handlerType) =>
        new SubscriptionInfo(false, handlerType);
}