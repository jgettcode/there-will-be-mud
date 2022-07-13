using System.Text.Json.Nodes;

namespace ThereWillBeMud;

// This class is a wrapper for the JsonObject returned by the API call.
public class OpenWeatherResponse
{
	private readonly JsonObject _jobj;
	
	public OpenWeatherResponse(JsonObject jobj)
	{
		_jobj = jobj;
	}

    public int StatusCode => Convert.ToInt32(GetNodeNullSafe(_jobj, "cod").GetValue<string>());
	
	public IEnumerable<OpenWeatherInfo> GetWeatherInfo()
	{
        var list = _jobj["list"];
        if (list == null)
            return new List<OpenWeatherInfo>();
        else
		    return list.AsArray().Select(CreateOpenWeatherInfo).ToList();
	}
	
	private OpenWeatherInfo CreateOpenWeatherInfo(JsonNode? jn)
	{
        if (jn == null)
            throw new ArgumentNullException("jn");
            
        var dt = UnixTimeStampToDateTime(GetNodeNullSafe(jn, "dt").GetValue<double>());
        var temp = GetNodeNullSafe(jn["main"], "temp").GetValue<double>();
        var tempMin = GetNodeNullSafe(jn["main"], "temp_min").GetValue<double>();
        var tempMax = GetNodeNullSafe(jn["main"], "temp_max").GetValue<double>();
        var rainVol3h = GetVolume3h(jn, "rain");
        var snowVol3h = GetVolume3h(jn, "snow");

		var result = new OpenWeatherInfo
		{
			Date = dt,
			TemperatureC = temp,
			TemperatureMinC = tempMin,
			TemperatureMaxC = tempMax,
			RainVolume3h = rainVol3h,
			SnowVolume3h = snowVol3h
		};
		
		return result;
	}

    private JsonNode GetNodeNullSafe(JsonNode? jn, string key)
    {
        if (jn == null)
            throw new ArgumentNullException("jn");

        JsonNode? node = jn[key];

        if (node == null)
            throw new Exception($"Missing property: {key}");

        return node;
    }
	
	private double GetVolume3h(JsonNode jn, string key)
	{
		// using this method because the property might not exist so we need to check for null
		if (jn[key] != null)
			return GetNodeNullSafe(jn[key], "3h").GetValue<double>();
		return 0;
	}

	//https://stackoverflow.com/questions/249760/how-can-i-convert-a-unix-timestamp-to-datetime-and-vice-versa
	private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
	{
		// Unix timestamp is seconds past epoch
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
		return dateTime;
	}
}
