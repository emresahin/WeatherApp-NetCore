using Core.Cache.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cache.Provider
{
	public class RedisCacheProvider : ICacheRepository
	{
		private readonly StackExchange.Redis.IDatabase _database;

		public RedisCacheProvider(IConnectionMultiplexer connectionMultiplexer)
		{
			_database = connectionMultiplexer.GetDatabase();
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			var serializedValue = JsonConvert.SerializeObject(value);
			await _database.StringSetAsync(key, serializedValue, expiration);
		}

		public async Task<T> GetAsync<T>(string key)
		{
			var value = await _database.StringGetAsync(key);
			return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(value);
		}

		public async Task RemoveAsync(string key)
		{
			await _database.KeyDeleteAsync(key);
		}
	}
}
