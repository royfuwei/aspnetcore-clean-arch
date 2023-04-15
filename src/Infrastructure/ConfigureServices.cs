using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using CleanArch.Infrastructure.Identity.Jwt;
using CleanArch.Infrastructure.Persistence.EFCore.Contexts;
using CleanArch.Infrastructure.Persistence.InMemory;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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

        services.AddSingleton<JwtHelpers>();

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

        service.AddScoped<Persistence.EFCore.WeatherForecastContextSeed>();

        return service;
    }
}