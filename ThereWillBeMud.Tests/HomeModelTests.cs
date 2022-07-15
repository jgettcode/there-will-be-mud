using Xunit.Abstractions;
using ThereWillBeMud.Models;

namespace ThereWillBeMud.Tests;

public class HomeModelTests : TestBase
{
    public HomeModelTests(ITestOutputHelper output) : base(output) {}

    [Fact]
    public void CanGetWillThereBeMud()
    {
        var model = new HomeModel
        {
            WeatherInfo = GetResponse().GetWeatherInfo()
        };
        
        var test = model.WillThereBeMud();

        // check the "is muddy" logic
        if (model.MaxTemperatureC() <= 0)
        {
            _output.WriteLine("It will not be muddy because the temperature will be below freezing.");
            Assert.False(test);
        }
        else
        {
            if (model.TotalRainVolume() + model.TotalSnowVolume() == 0)
            {
                _output.WriteLine("It will not be muddy because the temperature will be above freezing but there will be no rain or snow.");
                Assert.False(test);
            }
            else
            {
                _output.WriteLine("It be muddy because the temperature will be above freezing and there will be some rain or snow.");
                Assert.True(test);    
            }
        }
    }
}