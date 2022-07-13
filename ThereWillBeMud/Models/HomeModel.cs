namespace ThereWillBeMud.Models;

public class HomeModel
{
    public int DaysOffset { get; set; }= 3;

    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public IEnumerable<WeatherInfo> WeatherInfo { get; set; } = new List<WeatherInfo>();

    public DateTime GetFutureDate() => DateTime.Now.Date.AddDays(DaysOffset);

    public double TotalRainVolume() => GetFutureWeather().Sum(x => x.RainVolume3h);

    public double TotalSnowVolume() => GetFutureWeather().Sum(x => x.SnowVolume3h);

    public double MaxTemperatureC() => GetFutureWeather().Max(x => x.TemperatureMaxC);

    public double MaxTemperatureF() => MaxTemperatureC() * 1.8 + 32;

    public IEnumerable<WeatherInfo> GetFutureWeather()
    {
        var futureDate = GetFutureDate();
        var infos = WeatherInfo.Where(x => x.Date >= futureDate && x.Date < futureDate.AddDays(1)).ToList();
        return infos;
    }

    public bool WillThereBeMud()
    {
        var infos = GetFutureWeather();

		if (infos.Count() > 0)
		{
			var isMuddy = infos.Any(x => x.IsMuddy());
            return isMuddy;
		}
		else
		{
            throw new Exception($"No forecast data found on {GetFutureDate():M/d/yyyy}");
		}
    }
}