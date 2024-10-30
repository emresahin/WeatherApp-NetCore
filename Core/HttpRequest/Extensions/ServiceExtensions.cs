using Core.DataAccess.Interfaces;
using Core.HttpRequest.Interfaces;
using Core.HttpRequest.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpRequest.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection ServiceRequesterRegister(this IServiceCollection services, Type serviceRequestProvider)
		{
			services.AddScoped(typeof(IServiceRequest), serviceRequestProvider);
			services.AddScoped<ServiceRequestManager>();

			return services;
		}
	}
}
