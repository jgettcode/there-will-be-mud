using Xunit.Abstractions;

namespace ThereWillBeMud.Tests;

public class OpenWeatherClientTests
{
    private static string APIKEY = "aa10c577335449056c4db1a4bb52df73";
    private static string LOCATION = "Chelsea,MI,USA";
    
    private readonly OpenWeatherClient _client;
    private readonly ITestOutputHelper _output;

    public OpenWeatherClientTests(ITestOutputHelper output)
    {
        _client = new OpenWeatherClient(APIKEY);
        _output = output;
    }

    [Fact]
    public void Dispose()
    {
        _client.Dispose();
    }

    [Fact]
    public async Task CanGetOpenWeatherResponse()
    {
        var resp = await _client.GetResponseAsync(LOCATION);
        Assert.Equal(200, resp.StatusCode);
    }
}