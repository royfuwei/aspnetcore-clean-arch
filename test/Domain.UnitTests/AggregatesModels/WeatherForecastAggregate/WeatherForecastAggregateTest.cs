using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CleanArch.Domain.AggregatesModels.WeatherForecastAggregate;

namespace CleanArch.Domain.UnitTests.AggregatesModels.WeatherForecastAggregate
{
    public class WeatherForecastAggregateTest
    {
        [Fact]
        public void Create_weaherForecast_item_success()
        {
            var id = 1;
            var date = DateTime.Parse("2023/01/01");
            var temperatureC = 23;
            var summary = "warm";

            var result = new WeatherForecast(
                id, date, temperatureC, summary
            );
            Assert.NotNull(result);
        }
    }
}