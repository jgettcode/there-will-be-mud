using Microsoft.AspNetCore.Mvc;
using ThereWillBeMud.Controllers;
using ThereWillBeMud.Models;
using Xunit.Abstractions;

namespace ThereWillBeMud.Tests;

public class HomeControllerTests : TestBase
{
    public HomeControllerTests(ITestOutputHelper output) : base(output) {}

    [Fact]
    public void CanGetIndex()
    {
        var controller = new HomeController();
        var view = controller.Index() as ViewResult;
        Assert.NotNull(view);

        if (view != null)
        {
            Assert.NotNull(view.Model);
            if (view.Model != null)
            {
                var model = view.Model as HomeModel;
                Assert.NotNull(model);
                if (model != null)
                {
                    // should be empty during GET
                    Assert.Empty(model.WeatherInfo);
                }
            }
        }
    }

    [Fact]
    public async Task CanPostIndex()
    {
        var controller = new HomeController();
        var view = await controller.Index(new HomeModel { City = "Chelsea", State = "MI", Country = "USA" }) as ViewResult;
        Assert.NotNull(view);

        if (view != null)
        {
            Assert.NotNull(view.Model);
            if (view.Model != null)
            {
                var model = view.Model as HomeModel;
                Assert.NotNull(model);
                if (model != null)
                {
                    // should not be empty during POST
                    Assert.NotEmpty(model.WeatherInfo);
                    var test = model.WillThereBeMud();
                    _output.WriteLine("There will be mud in 3 days: {0}", test);
                }
            }
        }
    }
}