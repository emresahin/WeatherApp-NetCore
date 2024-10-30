using Business.Services.Interfaces;
using Business.Services.Models.WeatherStackResponseModels;
using Business.Services.Models;
using Core.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Business.Services.Models.WeatherApiResponseModels;
using Microsoft.Extensions.Logging;


namespace Business.Services.Provider
{
    public class WeatherApiProvider : IWeatherProvider
    {
        private readonly ServiceRequestManager _serviceRequestManager;
        private readonly ProviderApiInfo? _providerApiInfo;
        private readonly ILogger<WeatherApiProvider> _logger;
        public WeatherApiProvider(ServiceRequestManager serviceRequestManager, IConfiguration configuration, ILogger<WeatherApiProvider> logger)
        {
            _serviceRequestManager = serviceRequestManager;
            _providerApiInfo = configuration.GetSection("WeatherApiInfo").Get<ProviderApiInfo>();
            _logger = logger;


        }
        public async Task<decimal> GetCurrentWeatherByCityAsync(string cityName)
        {
            string path = $"{_providerApiInfo?.ApiUrl}/current.json?key={_providerApiInfo?.AccessKey}&q={cityName}&aqi=no";
            var response = await _serviceRequestManager.GetRequestAsync<WeatherApiResponseModel>(path, null, null, 5);
            if (response.IsSuccess && response.Result != null && response.Result.Current!=null)
            {
                return response.Result.Current.Temperature;
            }
            else
            {
                _logger.LogWarning("Hava durumu bilgisi alınamadı",cityName,response.Result);
                return 0;
            }
        }
    }
}
