using Core.HttpRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ExceptionHandler
{
    public class ExceptionHandlerBase
    {
        //Todo tek bir classta uygulayalum örnek açısından
        public async Task<ServiceRequestResult<T>> Execute<T>(Func<Task<T>> operation)
        {
            var response = new ServiceRequestResult<T>();

            try
            {
                response.Result = await operation();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
