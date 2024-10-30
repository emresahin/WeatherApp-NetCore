using Business.Models;
using Core.HttpRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DataAccess.Interfaces
{
    public interface IUserBusiness
    {
        public Task<ServiceRequestResult<UserModel>> LoginAsync(string email, string password);
        public Task<ServiceRequestResult<UserModel>> RegisterAsync(string email, string password);

        public Task<ServiceRequestResult<bool>> AddFavoriteCity(int userId, int cityId);
        public Task<ServiceRequestResult<bool>> RemoveFavoriteCity(int userId, int cityId);
        public Task<ServiceRequestResult<List<CityWeatherModel>>> GetFavoriteCities(int userId);
    }
}
