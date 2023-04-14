using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace CleanArch.Services.API;

public static class ConfigureServices
{
    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(options => 
        {
            options.SwaggerDoc("v1", new OpenApiInfo{
                Title = "CleanArch.Services.API",
                Description = "Simple ASP.NET Core Clean Arch",
                Version = "0.0.1"
            });

            // Add Jwt
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                }, new List<string>() }
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}