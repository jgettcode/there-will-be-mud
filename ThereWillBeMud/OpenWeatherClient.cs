using System.Text.Json.Nodes;

namespace ThereWillBeMud;

public class OpenWeatherClient : IDisposable
{
	private string _apikey;
	private HttpClient _hc;
	
	public OpenWeatherClient(string apikey)
	{
		_apikey = apikey;
		_hc = new HttpClient();
		_hc.BaseAddress = new Uri("https://api.openweathermap.org");
	}
	
	public async Task<OpenWeatherResponse> GetResponseAsync(string q)
	{
		var resp = await _hc.GetAsync($"data/2.5/forecast?q={q}&units=metric&appid={_apikey}");

		resp.EnsureSuccessStatusCode();
		
		var jobj = await resp.Content.ReadFromJsonAsync<JsonObject>();
		
        if (jobj == null)
            throw new Exception("API call returned null.");

		return new OpenWeatherResponse(jobj);
	}

    public void Dispose()
    {
        _hc.Dispose();
    }
}
