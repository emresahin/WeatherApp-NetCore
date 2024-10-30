using Business.DataAccess.Interfaces;
using Business.Models;
using Business.WeatherBusiness.Interfaces;
using Core.ExceptionHandler;
using Core.HttpRequest.Models;
using Core.Utility;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.DataAccess.Concrete
{
    public class UserBusiness :  IUserBusiness
    {
        private readonly RepositoryContextManager _repositoryContextManager;
        private readonly IWeatherBusiness _weatherBusiness;
        public UserBusiness(RepositoryContextManager repositoryContextManager, IWeatherBusiness weatherBusiness)
        {
            _repositoryContextManager = repositoryContextManager;
            _weatherBusiness = weatherBusiness;
        }

        public async Task<ServiceRequestResult<bool>> AddFavoriteCity(int userId, int cityId)
        {
            ServiceRequestResult<bool> result = new ServiceRequestResult<bool>();
            await _repositoryContextManager.Repository.AddAsync(new UserCityEntity { UserId = userId, CityId = cityId });
            result.IsSuccess = true;
            return result;
        }

        public async Task<ServiceRequestResult<List<CityWeatherModel>>> GetFavoriteCities(int userId)
        {
            ServiceRequestResult<List<CityWeatherModel>> result = new ServiceRequestResult<List<CityWeatherModel>>();

            var cityIds = await (from city in _repositoryContextManager.Repository.GetQueryable<CityEntity>()
                                 join userCity in _repositoryContextManager.Repository.GetQueryable<UserCityEntity>()
                                 on city.Id equals userCity.CityId
                                 where userCity.UserId == userId && userCity.IsActive
                                 select city.Id).ToListAsync();

            result.Result = await _weatherBusiness.GetCurrentWeatherByCityIdsAsync(cityIds);

            return result;
        }

        public async Task<ServiceRequestResult<UserModel>> LoginAsync(string email, string password)
        {
            ServiceRequestResult<UserModel> result = new ServiceRequestResult<UserModel>();
            string passwordHash = HashHelper.ComputeHash(password);
            var userEntity = await _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == email && p.Password == passwordHash && p.IsActive).FirstOrDefaultAsync();
            if (userEntity != null)
            {
                result.IsSuccess = true;
                result.Result = new UserModel { Id = userEntity.Id, Email = userEntity.Email, Password = userEntity.Password };
            }
            else
            {
                result.Message = "Kullanıcı bulunamadı";
            }

            return result;
        }

        public async Task<ServiceRequestResult<UserModel>> RegisterAsync(string email, string password)
        {
            ServiceRequestResult<UserModel> result = new ServiceRequestResult<UserModel>();
            string passwordHash = HashHelper.ComputeHash(password);

            var userEntityAlready = await _repositoryContextManager.Repository.GetQueryable<UserEntity>(p => p.Email == email && p.IsActive).FirstOrDefaultAsync();
            if (userEntityAlready != null)
            {
                result.IsSuccess = false;
                result.Message = "Belirtilen maile ait kullanıcı var";
            }
            else
            {
                var userEntity = await _repositoryContextManager.Repository.AddAsync(new UserEntity { Email = email, Password = passwordHash });
                result.Result = new UserModel { Id = userEntity.Id, Email = email, Password = passwordHash };
                result.IsSuccess = true;
            }

            return result;
        }

        public async Task<ServiceRequestResult<bool>> RemoveFavoriteCity(int userId, int cityId)
        {
            ServiceRequestResult<bool> result = new ServiceRequestResult<bool> { IsSuccess = true };
            var userCityList = await _repositoryContextManager.Repository.GetListAsync<UserCityEntity>(p => p.UserId == userId && p.CityId == cityId && p.IsActive, asNoTracking: false);
            foreach (var item in userCityList)
            {
                item.IsActive = false;
                await _repositoryContextManager.Repository.UpdateAsync(item);
            }

            return result;
        }
    }
}
