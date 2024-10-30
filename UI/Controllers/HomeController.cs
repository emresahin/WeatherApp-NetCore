using Business.DataAccess.Interfaces;
using Business.Services.Interfaces;
using Business.WeatherBusiness.Interfaces;
using Core.Cache;
using Core.HttpRequest;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UI.Models;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherBusiness _weatherBusiness;
        private readonly ICityBusiness _cityBusiness;
        public HomeController(IWeatherBusiness weatherBusiness, ICityBusiness cityBusiness, IConfiguration configuration)
        {

            _weatherBusiness = weatherBusiness;
            _cityBusiness = cityBusiness;
        }

        public async Task<IActionResult> Index()
        {

            var cityList = await _cityBusiness.GetCities();
            WeatherViewModel viewModel = new WeatherViewModel { Cities = cityList.Result };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<int> selectedValues)
        {
            var city = await _cityBusiness.GetCities();
            var weatherList = await _weatherBusiness.GetCurrentWeatherByCityIdsAsync(selectedValues);

            WeatherViewModel viewModel = new WeatherViewModel { Cities = city.Result, Weathers = weatherList };
            return View(viewModel);
        }


        public IActionResult Test()
        {
            int a = Convert.ToInt32("sdfsdf");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
