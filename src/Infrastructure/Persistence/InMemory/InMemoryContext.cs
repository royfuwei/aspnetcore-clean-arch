using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;
using CleanArch.Domain.SeedWork;
using CleanArch.Domain.SeedWork.Interfaces;
using MediatR;

namespace CleanArch.Infrastructure.Persistence.InMemory;
public class InMemoryContext : IUnitOfWork
{
    private readonly IMediator _mediator;

    public IEnumerable<WeatherForecast> WeatherForecasts = new List<WeatherForecast>();

    public InMemoryContext(
        IMediator mediator
    ) => _mediator = mediator;
    
    void IDisposable.Dispose()
    {
        return;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(1);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken)
    {
        var domainEvents = WeatherForecasts
            .SelectMany(e => e.DomainEvents)
            .ToList();
        WeatherForecasts.ToList().ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
        return true;
    }
}