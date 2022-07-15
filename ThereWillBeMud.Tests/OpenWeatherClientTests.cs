using Xunit.Abstractions;

namespace ThereWillBeMud.Tests;

public class OpenWeatherClientTests : TestBase
{    
    public OpenWeatherClientTests(ITestOutputHelper output) : base(output) {}

    [Fact]
    public void CanGetOpenWeatherResponse()
    {
        // make sure we got a success status code
        Assert.Equal(200, GetResponse().StatusCode);
    }

    [Fact]
    public void CanGetWeatherInfo()
    {
        var today = DateTime.Now.Date;
        var weatherInfo = GetResponse().GetWeatherInfo();

        // get the next day forecast to make sure we got some data
        var test = weatherInfo.FirstOrDefault(x => x.Date >= today);

        Assert.NotNull(test);
    }
}