using Business.DataAccess;
using Business.DataAccess.Interfaces;
using Business.Models;
using Business.Services.Interfaces;
using Business.Services.Models;
using Business.Services.Provider;
using Business.WeatherBusiness.Interfaces;
using Core.Cache;
using Core.HttpRequest;
using Core.HttpRequest.Models;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Business.WeatherBusiness.Concrete
{
    
    public class WeatherBusiness : IWeatherBusiness
    {
        private readonly CacheManager _cacheManager;
        private readonly IEnumerable<IWeatherProvider> _providers;
        private readonly RepositoryContextManager _repositoryContextManager;
        private readonly ICityBusiness _cityBusiness;

        private readonly IMemoryCache   _memoryCache;

        private readonly int delaySeconds;
        private readonly ILogger<WeatherBusiness> _logger;
        public WeatherBusiness(CacheManager cacheManager, IEnumerable<IWeatherProvider> providers, RepositoryContextManager repositoryContextManager, ICityBusiness cityBusiness, ILogger<WeatherBusiness> logger, IMemoryCache memoryCache,IConfiguration configuration)
        {
            _providers = providers;
            _cacheManager = cacheManager;
            _repositoryContextManager = repositoryContextManager;
            _cityBusiness = cityBusiness;
            _logger = logger;
            _memoryCache = memoryCache;
           
            delaySeconds = configuration.GetSection("SameRequestWaitTime").Get<int>(); 
        }
        public async Task<ServiceRequestResult<decimal>> GetCurrentWeatherByCityAsync(string cityName)
        {
            ServiceRequestResult<decimal> result = await GetCurrentWeatherFromCacheAsync(cityName);
            if (result.IsSuccess)
            {
                return result;
            }


            #region SemaphoreSlim

            SemaphoreSlim semaphore =  GetSemaphoreSlim(cityName);


            var startTime = DateTime.UtcNow;
            await semaphore.WaitAsync();
            try
            {

                result = await GetCurrentWeatherFromCacheAsync(cityName); // Diğer istekler geldiğinde burası dolmuş olabilir
                if (result.IsSuccess)
                {
                    return result;
                }

                // Kalan bekleme süresi hesaplanır
                var elapsed = (DateTime.UtcNow - startTime).TotalSeconds;
                var remainingDelay = Math.Max(0, delaySeconds - elapsed);

                if (remainingDelay > 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(remainingDelay));
                }

                decimal totalDegree = 0;
                int successCount = 0;
                decimal averagetDegree = 0;

                foreach (var item in _providers)
                {
                    decimal cityDegree = await item.GetCurrentWeatherByCityAsync(cityName);
                    if (cityDegree > 0)
                    {
                        successCount++;
                        totalDegree += cityDegree;
                    }
                }

                if (successCount == 0) // Her iki servis cevap dönmemiş demektir. (gereksinimlerde çakışma var kontrol edilmeli 2.maddenin son maddesi ile 4.maddenin ikinci maddesi
                {
                    result.IsSuccess = false;
                    result.Message = "Belirtilen şehir için hava durumu bilgisi bulunamadı...";
                    _logger.LogCritical("Tüm providerlar cevap dönmedi.");
                }
                else
                {
                    averagetDegree = totalDegree / successCount;

                    await _cacheManager.SetAsync(cityName, averagetDegree, TimeSpan.FromMinutes(10));
                    var cityModel = await _repositoryContextManager.Repository.GetQueryable<CityEntity>(p => p.Name == cityName).FirstOrDefaultAsync();
                    if (cityModel != null)
                    {
                        await _repositoryContextManager.Repository.AddAsync<WeatherCityEntity>(new WeatherCityEntity { CityId = cityModel.Id, Degree = averagetDegree });
                    }

                    result.IsSuccess = true;
                    result.Result = averagetDegree;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "İstek kuyruk hatası");
            }
            finally
            {
                semaphore.Release();
                await _cacheManager.RemoveAsync($"{cityName}_Semaphore");
            }



            return result;
            #endregion
        }

        public async Task<List<CityWeatherModel>> GetCurrentWeatherByCityIdsAsync(List<int> cityIds)
        {
            List<CityWeatherModel> result = new List<CityWeatherModel> { };
            var cityList = await _cityBusiness.GetCities();
            foreach (int item in cityIds)
            {
                var city = cityList.Result?.Find(p => p.Id == item);
                if (city != null)
                {
                    var cityWeatherResult = await GetCurrentWeatherByCityAsync(city.Name);
                    if (cityWeatherResult.IsSuccess)
                    {

                        result.Add(new CityWeatherModel { City = city, Degree = cityWeatherResult.Result });
                    }




                }
            }
            return result;
        }


        private async Task<ServiceRequestResult<decimal>> GetCurrentWeatherFromCacheAsync(string cityName)
        {
            ServiceRequestResult<decimal> result = new();
            decimal averagetDegree = 0;
            averagetDegree = await _cacheManager.GetAsync<decimal>(cityName); // İlk önce cache kontrolü yapalım
            if (averagetDegree == 0) // Cache üzerinde yoksa dbden getirelim
            {
                averagetDegree = await (from city in _repositoryContextManager.Repository.GetQueryable<CityEntity>()
                                        join weather in _repositoryContextManager.Repository.GetQueryable<WeatherCityEntity>()
                                        on city.Id equals weather.CityId
                                        where city.Name == cityName && weather.CreatedDate == DateTime.Now.Date
                                        select weather.Degree).FirstOrDefaultAsync();

                if (averagetDegree > 0)
                {
                    result.IsSuccess = true;
                    result.Result = averagetDegree;
                }
            }
            else
            {
                result.IsSuccess = true;
                result.Result = averagetDegree;

            }

            return result;
        }
        private  SemaphoreSlim GetSemaphoreSlim(string cityName)
        {
            ConcurrentDictionary<string, SemaphoreSlim>? semaphores = _memoryCache.Get<ConcurrentDictionary<string, SemaphoreSlim>>($"{cityName}_Semaphore");

            if (semaphores == null)
            {
                semaphores = new();
                var semaphore = semaphores.GetOrAdd(cityName, _ => new SemaphoreSlim(1, 1));
                 _memoryCache.Set($"{cityName}_Semaphore", semaphores);
                return semaphore;
            }
            else
            {
                var semaphore = semaphores.GetOrAdd(cityName, _ => new SemaphoreSlim(1, 1));
                return semaphore;
            }

        }

    }
}
