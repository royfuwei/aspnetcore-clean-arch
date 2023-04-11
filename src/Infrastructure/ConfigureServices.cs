using System.Reflection;
using CleanArch.Infrastructure.Persistence.InMemory;
using Microsoft.Extensions.DependencyInjection;

// namespace Microsoft.Extensions.DependencyInjection;
namespace CleanArch.Infrastructure;

// public static class ConfigureServices
public static class InfrastructureConfigureServices
{
    // public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
    public static IServiceCollection AddInfrastructureServices(IServiceCollection service)
    {
        // 原生DI MediatR
        service.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        service.AddSingleton(typeof(InMemoryContext));

        return service;
    }
}