using System.Reflection;
using CleanArch.Application.Behaviors;
using CleanArch.Application.Modules.WeatherForecasts.Repositories.EFcore;
using CleanArch.Application.Modules.WeatherForecasts.Repositories.InMemory;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate.Repositories;
using Microsoft.Extensions.DependencyInjection;
using CleanArch.Application.Modules.Jwt.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using CleanArch.Application.Modules.Identity;
using CleanArch.Application.Modules.Identity.UseCases;
using Microsoft.Extensions.Configuration;

namespace CleanArch.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration Configuration)
    {
        // 原生DI MediatR
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        // services.AddSingleton(typeof(IWeatherForecastRepository), typeof(InMemoryWeatherForecastRepository));
        services.AddScoped(typeof(IWeatherForecastRepository), typeof(EFCoreWeatherForecastRepository));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IdentityService>();
        services.AddSingleton<IJwtService, JwtService>();

        return services;
    }

    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // prevent from mapping "sub" claim to nameidentifier.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        var identityUrl = configuration.GetValue<string>("IdentityUrl");
        var signKey = configuration.GetValue<string>("JwtSettings:SignKey");
        var issuer = configuration.GetValue<string>("JwtSettings:Issuer");
        var audience = configuration.GetValue<string>("JwtSettings:Audience");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.IncludeErrorDetails = true; // 當驗證失敗時，會顯示失敗的詳細錯誤原因
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // 簽發者
                ValidateIssuer = true,
                ValidIssuer = issuer,
                // 接收者
                ValidateAudience = false,
                // ValidAudience = audience,
                // Token 的有效期間
                ValidateLifetime = true,
                // 如果 Token 中包含 key 才需要驗證，一般都只有簽章而已
                ValidateIssuerSigningKey = false,
                // key
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey!))
            };
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme);

            defaultAuthorizationPolicyBuilder =
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });
        return services;
    }
}