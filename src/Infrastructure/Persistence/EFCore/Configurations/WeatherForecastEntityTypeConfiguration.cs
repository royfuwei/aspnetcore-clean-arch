using CleanArch.Infrastructure.Persistence.EFCore.Contexts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArch.Infrastructure.Persistence.EFCore.Configurations;
public class WeatherForecastEntityTypeConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        // table & schema
        builder.ToTable("WeatherForecasts", WeatherForecastContext.DEFAULT_SCHEMA);
        
        // has key
        builder.HasKey(o => o.Id);

        // 不寫入db property
        builder.Ignore(o => o.DomainEvents);

        builder.Property(o => o.Id)
            .UseHiLo("weatherforecastseq");

        builder.Property<int>("TemperatureC")
            .IsRequired();

    }
}