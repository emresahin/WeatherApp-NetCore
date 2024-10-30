using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Models.WeatherStackResponseModels
{
    public class WeatherStackResponseModel
    {
        [JsonProperty("current")]
        public WeatherStackCurrent Current { get; set; }

        [JsonProperty("location")]
        public WeatherLocation Location { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("error")]
        public WeatherStackError Error { get; set; }

    }
    public class WeatherStackCurrent
    {
        [JsonProperty("temperature")]
        public int Temperature { get; set; }
    }

    public class WeatherStackError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("info")]
        public string Message { get; set; }
    }

}
