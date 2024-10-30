using Business.Models;
using Core.HttpRequest.Models;

namespace UI.Models
{
    public class WeatherViewModel
    {
        public List<CityModel> Cities { get; set; }
        public List<CityWeatherModel> Weathers { get; set; }
    }
}
