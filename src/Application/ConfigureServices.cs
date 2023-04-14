using System.Reflection;
using CleanArch.Application.Behaviors;
using CleanArch.Application.Modules.WeatherForecasts.Repositories.EFcore;
using CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection service, IConfiguration Configuration)
    {
        // 原生DI MediatR
        service.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        // service.AddSingleton(typeof(IWeatherForecastRepository), typeof(InMemoryWeatherForecastRepository));
        service.AddScoped(typeof(IWeatherForecastRepository), typeof(EFCoreWeatherForecastRepository));

        service.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        return service;
    }
}