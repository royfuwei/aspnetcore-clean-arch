using CleanArch.Infrastructure.Persistence.EFCore.Configurations;

namespace CleanArch.Infrastructure.Persistence.EFCore.Contexts;
public class WeatherForecastDbContext : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "weatherforecast";

    public DbSet<WeatherForecast>? WeatherForecasts { get; set; }

    private readonly IMediator _mediator;

    public WeatherForecastDbContext(
        IMediator mediator
    ) 
    {
        _mediator = mediator ?? throw new ArgumentNullException();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new WeatherForecastEntityTypeConfiguration());
    }

    /// <summary>
    /// SaveEntities dispatch domain event
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEventsAsync(this);
        var result = await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}