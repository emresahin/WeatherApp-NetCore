using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Models.WeatherApiResponseModels
{
    public class WeatherApiResponseModel
    {
        [JsonProperty("current")]
        public Current Current { get; set; }

        [JsonProperty("location")]
        public WeatherLocation Location { get; set; }


        [JsonProperty("error")]
        public WeatherApiError Error { get; set; }
    }


    public class Current
    {
        [JsonProperty("temp_c")]
        public decimal Temperature { get; set; }
    }

    public class WeatherApiError
    {
        [JsonProperty("code")]
        public string Code { get; set; }


        [JsonProperty("message")]
        public string Message { get; set; }

    }

}
