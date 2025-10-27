using Redis.Shared.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Redis.Shared.Services
{
    public class RedisCache : IRedisCache
    {
        private readonly IDatabase _database;
        public RedisCache(IConnectionMultiplexer multiplexer)
        {
            _database = multiplexer.GetDatabase();
        }
        public async Task<T?> GetDataAsync<T>(string key)
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value!);
        }

        public Task<bool> RemoveAsync(string key)
        {
            return _database.KeyDeleteAsync(key);
        }

        public Task<bool> SetDataAsync<T>(string key, T value)
        {
           var json = JsonSerializer.Serialize(value);
           return _database.StringSetAsync(key, json);
        }
    }
}
