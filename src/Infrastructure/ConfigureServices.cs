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
    
}