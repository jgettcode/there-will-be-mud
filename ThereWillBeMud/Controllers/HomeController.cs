using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ThereWillBeMud.Models;

namespace ThereWillBeMud.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        // using these defaults, this should be populated in a better way...
        var model = new HomeModel { City = "Ann Arbor", State = "MI", Country = "USA" };
        ViewBag.Error = string.Empty;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Index(HomeModel model)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(model.City))
            error = "City is required.";
        if (string.IsNullOrEmpty(model.Country))
            error = "Country is required.";
        
        if (string.IsNullOrEmpty(error))
        {
            // the apikey should be stored in a config file or something...
            var wc = new OpenWeatherClient("aa10c577335449056c4db1a4bb52df73");

            var query = string.Join(",", model.City, model.State, model.Country);
            var resp = await wc.GetResponseAsync(query);
            
            if (resp.StatusCode == 200)
                model.WeatherInfo = resp.GetWeatherInfo();
            else
                error = resp.Message;
        }

        ViewBag.Error = error;

        return View(model);
    }
}
