using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpRequest.Models
{
	public class ServiceRequestResult<T>
	{
		public bool IsSuccess { get; set; }
		public string Code { get; set; }
		public string Message { get; set; }
		public string MessageCode { get; set; }
		public T Result { get; set; }
	}
}
