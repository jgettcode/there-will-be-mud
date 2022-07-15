using System.Text.Json;
using System.Text.Json.Nodes;

namespace ThereWillBeMud;

// This class is a wrapper for the JsonObject returned by the API call.
public class OpenWeatherResponse
{
	private readonly JsonObject? _jobj;
	
	public OpenWeatherResponse() {}

	public OpenWeatherResponse(JsonObject jobj)
	{
		_jobj = jobj;
	}

    public int StatusCode
	{
		get
		{
			// sometimes the API returns a number as string (e.g. "200") or a number (e.g. 401)
			// so this code handles either case, and always returns int

			int result = 0;

			JsonElement je = GetNodeNullSafe(_jobj, "cod").GetValue<JsonElement>();

			if (je.ValueKind == JsonValueKind.Number)
			{
				result = je.GetInt32();
			}
			else
			{
				var s = je.GetString();
				if (s != null)
					result = Convert.ToInt32(s);
			}

			return result;
		}
	}
	

	public string Message
	{
		get
		{
			string result = string.Empty;

			JsonElement je = GetNodeNullSafe(_jobj, "message").GetValue<JsonElement>();
			
			if (je.ValueKind == JsonValueKind.Number)
			{
				var v = je.GetInt32();
				if (v != 0)
					result = v.ToString();
			}
			else
			{
				var s = je.GetString();
				if (s != null)
					result = s;
			}

			return result;
		}
	}

	
	public IEnumerable<WeatherInfo> GetWeatherInfo()
	{
        var list = _jobj["list"];
        if (list == null)
            return new List<WeatherInfo>();
        else
		    return list.AsArray().Select(CreateOpenWeatherInfo).ToList();
	}
	
	private WeatherInfo CreateOpenWeatherInfo(JsonNode? jn)
	{
        if (jn == null)
            throw new ArgumentNullException("jn");
            
        var dt = UnixTimeStampToDateTime(GetNodeNullSafe(jn, "dt").GetValue<double>());
        var temp = GetNodeNullSafe(jn["main"], "temp").GetValue<double>();
        var tempMin = GetNodeNullSafe(jn["main"], "temp_min").GetValue<double>();
        var tempMax = GetNodeNullSafe(jn["main"], "temp_max").GetValue<double>();
        var rainVol3h = GetVolume3h(jn, "rain");
        var snowVol3h = GetVolume3h(jn, "snow");

		var result = new WeatherInfo
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
