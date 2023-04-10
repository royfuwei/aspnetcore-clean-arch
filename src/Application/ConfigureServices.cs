using System.Reflection;
using CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using Microsoft.Extensions.DependencyInjection;

// namespace Microsoft.Extensions.DependencyInjection;
namespace CleanArch.Application;

// public static class ConfigureServices
public static class ApplicationConfigureServices
{
    // public static IServiceCollection AddApplicationServices(this IServiceCollection service)
    public static IServiceCollection AddApplicationServices(IServiceCollection service)
    {
        // 原生DI MediatR
        service.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        service.AddSingleton(typeof(IWeatherForecastRepository), typeof(InMemoryWeatherForecastRepository));
        // service.AddScoped(typeof(IWeatherForecastRepository), typeof(InMemoryWeatherForecastRepository));

        // InMemoryWeatherForecastRepository.InitialData();
        return service;
    }
}