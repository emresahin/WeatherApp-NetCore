using Business.Models;
using Core.HttpRequest.Models;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DataAccess.Interfaces
{
    public interface ICityBusiness
    {
        public  Task<ServiceRequestResult<List<CityModel>>> GetCities();
    }
}
