using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Models
{
	public class WeatherLocation
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("country")]
		public string Country { get; set; }


		[JsonProperty("region")]
		public string Region { get; set; }

		[JsonProperty("lat")]
		public string Lat { get; set; }

		[JsonProperty("lon")]
		public string Lon { get; set; }
	}
}
