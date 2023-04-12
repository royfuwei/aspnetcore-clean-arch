namespace CleanArch.Infrastructure.Persistence.EFCore;

static class MediatorExtension
{
    /// <summary>
    /// Extension DbContext 接收 domain event Publish
    /// IMediator 可以直接使用
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<EntityBase>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
