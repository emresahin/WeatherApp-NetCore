using Business.Services.Interfaces;
using Business.Services.Models;
using Business.Services.Models.WeatherStackResponseModels;
using Core.HttpRequest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Provider
{
    public class WeatherStackProvider : IWeatherProvider
    {
        private readonly ServiceRequestManager _serviceRequestManager;
        private readonly ProviderApiInfo? _providerApiInfo;
        private readonly ILogger<WeatherStackProvider> _logger;
        public WeatherStackProvider(ServiceRequestManager serviceRequestManager, IConfiguration configuration, ILogger<WeatherStackProvider> logger)
        {
            _serviceRequestManager = serviceRequestManager;
            _providerApiInfo = configuration.GetSection("WeatherStackInfo").Get<ProviderApiInfo>();
            _logger = logger;

        }
        public async Task<decimal> GetCurrentWeatherByCityAsync(string cityName)
        {
            string path = $"{_providerApiInfo?.ApiUrl}/current?access_key={_providerApiInfo?.AccessKey}&query={cityName}";
            var response = await _serviceRequestManager.GetRequestAsync<WeatherStackResponseModel>(path, null, null, 5);
            if (response.IsSuccess && response.Result != null && response.Result.Current!=null)
            {
                return response.Result.Current.Temperature;
            }
            else
            {
                _logger.LogWarning("Hava durumu bilgisi alınamadı", cityName, response.Result);
                return 0;
            }
        }
    }
}
