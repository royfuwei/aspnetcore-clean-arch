using System.Reflection;
using CleanArch.Application.Modules.Identity;
using CleanArch.Application.Modules.WeatherForecasts.Repositories.EFcore;
using CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
using CleanArch.Domain.IntegrationEvents.Interfaces;
using CleanArch.Infrastructure.EventBus.InMemory;
using CleanArch.Infrastructure.Persistence.EFCore.Contexts;
using CleanArch.Infrastructure.Persistence.InMemory;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CleanArch.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 原生DI MediatR
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddSingleton(typeof(IInMemoryContext), typeof(InMemoryContext));

        return services;
    }

    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        // 先用 EventBusInMemory 測試，之後再加入EventBusRabbitMQ
        services.AddSingleton<IEventBus, EventBusInMemory>();
        services.AddSingleton<InMemoryEventBusSubscriptionsManager>();
        return services;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        if (bool.Parse(configuration["UseInMemoryDatabase"]!))
        {
            service.AddDbContext<WeatherForecastContext>(options => {
                options.UseInMemoryDatabase("CleanArchDb");
            }, ServiceLifetime.Scoped);
            service.AddDbContext<IdentityContext>(options => {
                options.UseInMemoryDatabase("CleanArchDb");
            }, ServiceLifetime.Scoped);
        } 
        else
        {
            service.AddDbContext<WeatherForecastContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(WeatherForecastContext).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                },
                ServiceLifetime.Scoped
            );

            service.AddDbContext<IdentityContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                },
                ServiceLifetime.Scoped
            );
        }

        service.AddScoped<IWeatherForecastContext, WeatherForecastContext>();

        service
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();

        service.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, IdentityContext>();


        
        service.AddScoped<Persistence.EFCore.WeatherForecastContextSeed>();
        service.AddScoped<Persistence.EFCore.IdentityContextSeed>();

        return service;
    }
}