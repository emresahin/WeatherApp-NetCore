using Core.Cache.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cache.Provider
{
	public class MemoryCacheProvider : ICacheRepository
	{
		private readonly IMemoryCache _memoryCache;

		public MemoryCacheProvider(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			var options = new MemoryCacheEntryOptions();
			if (expiration.HasValue)
				options.SetAbsoluteExpiration(expiration.Value);

			_memoryCache.Set(key, value, options);
			return Task.CompletedTask;
		}

		public Task<T> GetAsync<T>(string key)
		{
			return Task.FromResult(_memoryCache.TryGetValue(key, out T value) ? value : default);
		}

		public Task RemoveAsync(string key)
		{
			_memoryCache.Remove(key);
			return Task.CompletedTask;
		}
	}
}
