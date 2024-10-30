using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Interfaces
{
	public interface IWeatherProvider
	{
		public  Task<decimal> GetCurrentWeatherByCityAsync(string cityName);
	}
}
