using CleanArch.Services.API.Infrastructure.ActionResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CleanArch.Services.API.Infrastructure.Filters;
public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ApiExceptionFilterAttribute> _logger;
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute(
        IWebHostEnvironment env, ILogger<ApiExceptionFilterAttribute> logger
    )
    {
        _env = env;
        _logger = logger;
        // Register known exception types and handlers.
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        Type type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }
        HandleInternalServerErrorException(context);
        return;
    }

    private void HandleInternalServerErrorException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "https://developer.mozilla.org/docs/Web/HTTP/Status/500",
            Type = "InternalServerError",
            Instance = context.HttpContext.Request.Path,
        };

        context.Result = new InternalServerErrorObjectResult(details);

        if (_env.IsDevelopment())
        {
            details.Detail = context.Exception.ToString();
        }

        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "https://developer.mozilla.org/docs/Web/HTTP/Status/401",
            Type = "Unauthorized",
            Instance = context.HttpContext.Request.Path,
        };

        context.Result = new UnauthorizedObjectResult(details);

        if (_env.IsDevelopment())
        {
            details.Detail = context.Exception.ToString();
        }

        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "https://developer.mozilla.org/docs/Web/HTTP/Status/400",
            Type = "Invalid",
            Instance = context.HttpContext.Request.Path,
            Detail = "Please refer to the errors property for additional details."
        };

        if (_env.IsDevelopment())
        {
            details.Detail = context.Exception.ToString();
        }

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }
}