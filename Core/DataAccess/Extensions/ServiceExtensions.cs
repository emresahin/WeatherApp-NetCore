using Core.DataAccess.Concrete;
using Core.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection DBAccessRegister<TDbContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TDbContext : DbContext
		{
			services.AddDbContext<TDbContext>(optionsAction, contextLifetime: lifetime);
			services.Add(new ServiceDescriptor(
			   typeof(IRepository<TDbContext>),
			   serviceProvider =>
			   {
				   TDbContext dbContext = ActivatorUtilities.CreateInstance<TDbContext>(serviceProvider);
				   return new Repository<TDbContext>(dbContext);
			   },
			   lifetime));

			return services;
		}

	}
}
