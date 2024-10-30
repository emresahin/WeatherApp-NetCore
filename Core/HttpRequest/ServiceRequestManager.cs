using Core.HttpRequest.Interfaces;
using Core.HttpRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpRequest
{
	public  class ServiceRequestManager
	{
		private readonly IServiceRequest _serviceRequester;
		public ServiceRequestManager(IServiceRequest serviceRequester)
		{
			_serviceRequester = serviceRequester;

		}
		public async Task<ServiceRequestResult<T>> GetRequestAsync<T>(string path, object? data, Dictionary<string, string>? headers, int timeOut = 0)
		{
			return  await _serviceRequester.GetRequestAsync<T>(path,data,headers,timeOut);
		}

		public async Task<ServiceRequestResult<T>> PostRequestAsync<T>(string path, object data, Dictionary<string, string>? headers, int timeOut = 0)
		{
			return await _serviceRequester.PostRequestAsync<T>(path, data, headers, timeOut);
		}
	}
}
