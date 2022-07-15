using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThereWillBeMud.Models;

namespace ThereWillBeMud.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        var model = new HomeModel { City = "Chelsea", State = "MI", Country = "USA" };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(HomeModel model)
    {
        if (string.IsNullOrEmpty(model.City))
            throw new Exception("City is required");
        if (string.IsNullOrEmpty(model.Country))
            throw new Exception("Country is required");

        // this should be stored in a config file...
        var wc = new OpenWeatherClient("aa10c577335449056c4db1a4bb52df73");
        var query = string.Join(",", model.City, model.State, model.Country);
        var resp = await wc.GetResponseAsync(query);
        
        if (resp.StatusCode != 200)
            throw new Exception($"Bad status code from API: {resp.StatusCode}");

        model.WeatherInfo = resp.GetWeatherInfo();

        return View(model);
    }
}
