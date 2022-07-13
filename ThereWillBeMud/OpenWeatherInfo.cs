namespace ThereWillBeMud;

// This class stores weather data for each 3hr period returned by the API call.
public class OpenWeatherInfo
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

public static class OpenWeatherInfoExtensions
{
    /// <summary>
    /// Checks if there will be mud n days from today.
    /// </summary>
    public static bool WillThereBeMud(this IEnumerable<OpenWeatherInfo> source, int n)
    {
        var today = DateTime.Now.Date;
        var futureDate = today.AddDays(n);

        var infos = source.Where(x => x.Date >= futureDate && x.Date < futureDate.AddDays(1)).ToList();

		if (infos.Count > 0)
		{
			var isMuddy = infos.Any(x => x.IsMuddy());
            return isMuddy;
		}
		else
		{
            throw new Exception($"No forecast data found on {futureDate:M/d/yyyy}");
		}
    }
}
