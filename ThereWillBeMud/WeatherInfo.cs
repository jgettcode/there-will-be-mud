namespace ThereWillBeMud;

// This class stores weather data for each 3hr period returned by the API call.
public class WeatherInfo
{
	public DateTime Date { get; set; }
	public double TemperatureC { get; set; }
	public double TemperatureMinC { get; set; }
	public double TemperatureMaxC { get; set; }
	public double RainVolume3h { get; set; }
	public double SnowVolume3h { get; set; }
	
	public bool IsMuddy()
	{
		return TemperatureMaxC > 0
			&& (RainVolume3h > 0 || SnowVolume3h > 0);
	}
}
