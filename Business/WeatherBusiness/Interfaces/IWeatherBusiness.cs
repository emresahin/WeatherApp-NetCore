using Business.Models;
using Core.HttpRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.WeatherBusiness.Interfaces
{
	public interface IWeatherBusiness
	{
		public Task<ServiceRequestResult<decimal>> GetCurrentWeatherByCityAsync(string cityName);
        public Task<List<CityWeatherModel>> GetCurrentWeatherByCityIdsAsync(List<int> cityIds);
	}
}
