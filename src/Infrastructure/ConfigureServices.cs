using System.Reflection;
using CleanArch.Infrastructure.Persistence.EFCore.Contexts;
using CleanArch.Infrastructure.Persistence.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArch.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection service, IConfiguration configuration)
    {
        // 原生DI MediatR
        service.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        service.AddSingleton(typeof(InMemoryContext));

        return service;
    }

    public static IServiceCollection AddCustomDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        if (bool.Parse(configuration["UseInMemoryDatabase"]!))
        {
            service.AddDbContext<WeatherForecastDbContext>(options => {
                options.UseInMemoryDatabase("CleanArchDb");
            });
        } 
        else
        {
            service.AddDbContext<WeatherForecastDbContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(WeatherForecastDbContext).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
            });
        }

        return service;
    }
    
}