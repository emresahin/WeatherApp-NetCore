using Core.HttpRequest.Interfaces;
using Core.HttpRequest;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Cache.Interfaces;
using Core.Cache.Provider;
using StackExchange.Redis;

namespace Core.Cache.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection RegisterCacheProvider(this IServiceCollection services, Type cacheProvider)
		{
			if (cacheProvider.GetType == typeof(RedisCacheProvider).GetType)
			{
				services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
			}
			else if (cacheProvider.GetType == typeof(MemoryCacheProvider).GetType)
			{
				services.AddMemoryCache();
			}

			services.AddSingleton(typeof(ICacheRepository), cacheProvider);
			services.AddSingleton<CacheManager>();

			return services;
		}
		
	}
}
