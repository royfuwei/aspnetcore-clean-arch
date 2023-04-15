using CleanArch.Infrastructure.Identity;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;

namespace CleanArch.Infrastructure.Persistence.EFCore.Contexts;
public class IdentityContext : ApiAuthorizationDbContext<ApplicationUser>, IUnitOfWork
{
    private readonly IMediator _mediator;

    public IdentityContext(
        DbContextOptions<IdentityContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator
    ) : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
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