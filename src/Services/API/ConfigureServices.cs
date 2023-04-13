using CleanArch.Infrastructure.Persistence.EFCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace API;

public static class ConfigureServices
{

    public static IServiceCollection AddCustomDbContext(this IServiceCollection service, IConfiguration configuration)
    {
        if (bool.Parse(configuration["UseInMemoryDatabase"]!))
        {
            service.AddDbContext<WeatherForecastContext>(options => {
                options.UseInMemoryDatabase("CleanArchDb");
            }, ServiceLifetime.Scoped);
        } 
        else
        {
            service.AddDbContext<WeatherForecastContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(ConfigureServices).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                },
                ServiceLifetime.Scoped
            );
        }

        return service;
    }
    
}