using Core.HttpRequest.Interfaces;
using Core.HttpRequest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpRequest.Provider
{
	public class RestClientProvider : IServiceRequest
	{
		
		public async Task<ServiceRequestResult<T>> GetRequestAsync<T>(string path, object? data, Dictionary<string, string>? headers, int timeOut = 0)
		{
			RestClientOptions restClientOptions = new();
			restClientOptions.Timeout = TimeSpan.FromSeconds(timeOut);
			restClientOptions.BaseUrl = new(path);
			RestClient client = new(restClientOptions);
			RestRequest restR = new(path);

			if (data != null)
			{
				string body = SerializeObject(data);
				restR.AddJsonBody(body);
			}

			if (headers!=null)
			{
				foreach (var item in headers)
				{
                    restR.AddHeader(item.Key,item.Value);
                }

            }

			RestResponse response = await client.ExecuteGetAsync(restR);

			ServiceRequestResult<T> result = GetResponseResult<T>(response);
			return result;

		}

		public async Task<ServiceRequestResult<T>> PostRequestAsync<T>(string path, object data, Dictionary<string, string>? headers, int timeOut = 0)
		{
			RestClientOptions restClientOptions = new();
			restClientOptions.Timeout = TimeSpan.FromSeconds(timeOut);
			restClientOptions.BaseUrl = new Uri(path);
			RestClient client = new(restClientOptions);
			RestRequest restR = new(path);

			string body = SerializeObject(data);
			restR.AddJsonBody(body);

            if (headers != null)
            {
                foreach (var item in headers)
                {
                    restR.AddHeader(item.Key, item.Value);
                }

            }

            RestResponse response = await client.ExecutePostAsync(restR);

			ServiceRequestResult<T> result = GetResponseResult<T>(response);
			return result;


		}

		private static string SerializeObject(object data)
		{
			return JsonConvert.SerializeObject(data, new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				DateTimeZoneHandling = DateTimeZoneHandling.Local
			});
		}

		private ServiceRequestResult<T> GetResponseResult<T>(RestResponse response)
		{
			ServiceRequestResult<T> result = new();
			if (response.IsSuccessful)
			{
				var responseValue = JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
				result.IsSuccess = true;
				result.Code = response.StatusCode.ToString();
				result.Result = responseValue;
				return result;
			}

			else
			{
				if (!string.IsNullOrEmpty(response.Content))
				{
					var errorResponseValue = JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

					result.IsSuccess = false;
					result.Message = response.ErrorMessage;
					result.Code = response.StatusCode.ToString();
					result.Result = errorResponseValue;
					return result;
				}
				else
				{
					result.Code = response.StatusCode.ToString();
					result.Message = $"ErrorMessage: {response.ErrorMessage},Message: {response.ErrorException?.Message},InnerException: {response.ErrorException?.InnerException},StackTrace: {response.ErrorException?.StackTrace}";
				}

				return result;
			}
		}
	}
}
