
using Business.DataAccess.Interfaces;
using Business.Models;
using Core.Cache;
using Core.ExceptionHandler;
using Core.HttpRequest.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DataAccess.Concrete
{
    public class CityBusiness : ExceptionHandlerBase, ICityBusiness
    {
        private readonly RepositoryContextManager _repositoryContextManager;
        public readonly CacheManager _cacheManager;
        public CityBusiness(RepositoryContextManager repositoryContextManager, CacheManager cacheManager)
        {
            _repositoryContextManager = repositoryContextManager;
            _cacheManager = cacheManager;
        }

       

        public async Task<ServiceRequestResult<List<CityModel>>> GetCities()
        {

            return await Execute<List<CityModel>>(async () =>
              {
                  List<CityModel> cityList = await _cacheManager.GetAsync<List<CityModel>>("CityList");
                  if (cityList != null && cityList.Count > 0)
                  {
                      return cityList;
                  }

                  List<CityEntity> cityEntityList = _repositoryContextManager.Repository.GetQueryable<CityEntity>(p => p.IsActive).ToList();
                  if (cityEntityList.Count > 0)
                  {
                      cityList = cityEntityList.Select(p => new CityModel { Id = p.Id, Name = p.Name }).ToList();
                      await _cacheManager.SetAsync("CityList", cityList, TimeSpan.FromHours(12));
                  }

                  return cityList;
              });
        }


    }
}
