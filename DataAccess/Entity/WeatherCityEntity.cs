using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class WeatherCityEntity : BaseEntity
	{
		public int CityId { get; set; }
		public decimal Degree { get; set; }
		public CityEntity City { get; set; }

	}
}
