using System.Reflection;

namespace CleanArch.Application.Behaviors;
public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private ILogger<AuthorizationBehavior<TRequest, TResponse>> _logger;
    
    public AuthorizationBehavior(
        ILogger<AuthorizationBehavior<TRequest, TResponse>> logger
    ) {
        _logger = logger;
    }
    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handle: AuthorizationBehavior");
        return next();
        // throw new NotImplementedException();
    }
}