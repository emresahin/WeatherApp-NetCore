using Core.HttpRequest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpRequest.Interfaces
{
	public interface IServiceRequest
	{
		public Task<ServiceRequestResult<T>> PostRequestAsync<T>(string path, object data, Dictionary<string, string>? headers, int timeOut = 0);
		public Task<ServiceRequestResult<T>> GetRequestAsync<T>(string path, object? data, Dictionary<string, string>? headers, int timeOut = 0);

	}
}
