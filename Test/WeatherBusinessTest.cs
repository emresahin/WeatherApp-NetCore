using Business.DataAccess.Concrete;
using Business.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.WeatherBusiness.Concrete;
using Business.WeatherBusiness.Interfaces;
using Business.Services.Provider;
using Core.Cache;

namespace Test
{
    public class WeatherBusinessTest : BaseTest
    {
        private IWeatherBusiness _weatherBusiness;
        private CacheManager _cacheManager;
        [SetUp]
        public void Setup()
        {
            _weatherBusiness = ServiceProvider.GetRequiredService<IWeatherBusiness>();
            _cacheManager = ServiceProvider.GetRequiredService<CacheManager>();
        }


        [Test]
        public async Task Test_GetWeatherCityNonCache()
        {
            await _cacheManager.RemoveAsync("Ankara");
            var result = await _weatherBusiness.GetCurrentWeatherByCityAsync("Ankara");

            Assert.AreEqual(true, result.IsSuccess);
        }

        [Test]
        public async Task Test_GetWeatherCityFromCache()
        {
            await _cacheManager.SetAsync("Ankara", 5);

            var result = await _weatherBusiness.GetCurrentWeatherByCityAsync("Ankara");
            Assert.IsTrue(result.Result == 5);
        }

        [Test]
        public async Task Test_GetWeatherCityForMultipleRequest()
        {
            await _cacheManager.RemoveAsync("Ankara");

            DateTime startRequest = DateTime.Now;
            var taskList = new List<Task>();
            taskList.Add(_weatherBusiness.GetCurrentWeatherByCityAsync("Ankara"));
            
            await Task.Delay(TimeSpan.FromSeconds(4));
            taskList.Add(_weatherBusiness.GetCurrentWeatherByCityAsync("Ankara"));

            await Task.Delay(TimeSpan.FromSeconds(4));
            taskList.Add(_weatherBusiness.GetCurrentWeatherByCityAsync("Ankara"));



            await Task.WhenAll(taskList);
            DateTime endRequest = DateTime.Now;
            TimeSpan difference = (endRequest - startRequest);
            
            Assert.IsTrue(difference.TotalSeconds < 25);
        }
    }
}
