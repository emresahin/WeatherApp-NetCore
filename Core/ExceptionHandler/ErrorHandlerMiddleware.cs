using Core.HttpRequest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.ExceptionHandler
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                _logger.LogCritical($"Uygulama hatası :{error.Message} ");
                //slack message veya farklı bir işlem yapılabilir
                //hata durumua göre status code değişiklik gösterebilir 
                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ServiceRequestResult<string> result = new ServiceRequestResult<string>();
                result.Message = error.Message;


                await response.WriteAsync(JsonSerializer.Serialize(result));
            }
        }
    }
}
