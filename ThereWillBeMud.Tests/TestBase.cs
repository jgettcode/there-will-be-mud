using Xunit.Abstractions;

namespace ThereWillBeMud.Tests;

public abstract class TestBase : IAsyncLifetime
{
    private static string APIKEY = "aa10c577335449056c4db1a4bb52df73";
    private static string LOCATION = "Chelsea,MI,USA";
    
    protected readonly ITestOutputHelper _output;
    protected readonly OpenWeatherClient _client;
    private OpenWeatherResponse? _resp;

    protected OpenWeatherResponse GetResponse()
    {
        // make sure the response is not null
        if (_resp == null) throw new Exception("API response is null.");
        return _resp;
    }

    public TestBase(ITestOutputHelper output)
    {
        _output = output;
        _client = new OpenWeatherClient(APIKEY);
    }

    [Fact]
    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task InitializeAsync()
    {
        // make one API call for all tests
        _resp = await _client.GetResponseAsync(LOCATION);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}