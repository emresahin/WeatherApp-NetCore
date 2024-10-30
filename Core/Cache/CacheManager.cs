using Core.Cache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cache
{
	public class CacheManager
	{
		private readonly ICacheRepository _cacheRepository;

		public CacheManager(ICacheRepository cacheRepository)
		{
			_cacheRepository = cacheRepository;
		}

		public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			return _cacheRepository.SetAsync(key, value, expiration);
		}

		public Task<T> GetAsync<T>(string key)
		{
			return _cacheRepository.GetAsync<T>(key);
		}

		public Task RemoveAsync(string key)
		{
			return _cacheRepository.RemoveAsync(key);
		}
	}
}
