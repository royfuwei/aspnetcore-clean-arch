using CleanArch.Application;
using CleanArch.Application.Modules.WeatherForecasts.EventHandlers.IntegrationEventHandlers;
using CleanArch.Domain.IntegrationEvents.Interfaces;
using CleanArch.Infrastructure;
using CleanArch.Infrastructure.Persistence.EFCore;
using CleanArch.Services.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddCustomAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorization(builder.Configuration);
builder.Services.AddCustomSwaggerGen(builder.Configuration);
builder.Services.AddAPIServices(builder.Configuration);
builder.Services.AddInMemoryEventBus(builder.Configuration);


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<WeatherForecastContextSeed>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
        var identityInitialiser = scope.ServiceProvider.GetRequiredService<IdentityContextSeed>();
        await identityInitialiser.InitialiseAsync();
        if (builder.Configuration.GetValue<bool>("IdentityService:UseIdentityDefaultUser"))
        {
            var username = builder.Configuration.GetValue<string>("IdentityService:DefaultUser:UserName");
            var email = builder.Configuration.GetValue<string>("IdentityService:DefaultUser:Email");
            var password = builder.Configuration.GetValue<string>("IdentityService:DefaultUser:Password");
            await identityInitialiser.SeedAsync(username, email, password);
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

ConfigureEventBus(app);

app.MapControllers();

app.Run();


/// <summary>
/// 取得eventBus，並訂閱IntegrationEvent
/// </summary>
/// <param name="app"></param>
void ConfigureEventBus(IApplicationBuilder app)
{
    var eventBus = app.ApplicationServices.GetRequiredService<CleanArch.Domain.IntegrationEvents.Interfaces.IEventBus>();
    eventBus.Subscribe<WeatherForecastTestIEvent, IIntegrationEventHandler<WeatherForecastTestIEvent>>();
}