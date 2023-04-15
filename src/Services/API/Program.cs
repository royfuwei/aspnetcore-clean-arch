using CleanArch.Application;
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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

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

app.MapControllers();

app.Run();
