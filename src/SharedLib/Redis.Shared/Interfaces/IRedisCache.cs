using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redis.Shared.Interfaces
{
    public interface IRedisCache
    {
        Task<T> GetDataAsync<T>(string key);
        Task<bool> SetDataAsync<T>(string key, T value);
        Task<bool> RemoveAsync(string key);
    }
}
