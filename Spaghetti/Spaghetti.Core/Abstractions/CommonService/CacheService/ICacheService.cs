using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaghetti.Core.Abstractions.CommonService.CacheService
{
    public interface ICacheService
    {
        #region Common
        bool Remove(string key);
        Task<bool> RemoveAsync(string key);
        bool KeyExists(string key);
        Task<bool> KeyExistsAsync(string key);
        bool DeleteKey(string key);
        Task<bool> DeleteKeyAsync(string key);
        bool SetExpire(string key, TimeSpan? timespan);
        Task<bool> SetExpireAsync(string key, TimeSpan? timespan);

        bool Lock(string key, string value, TimeSpan exprity = default(TimeSpan));
        Task<bool> LockAsync(string key, string value, TimeSpan expiry = default(TimeSpan));
        bool Unlock(string key, string value);
        Task<bool> UnlockAsync(string key, string value);

        #endregion

        #region get and set
        T Get<T>(string key);
        Task<T> GetAsync<T>(string key);
        bool Set<T>(string key, T value);
        bool Set<T>(string key, T value, TimeSpan expiresIn);
        Task<bool> SetAsync<T>(string key, T value, TimeSpan expiresIn);
        string GetString(string key);
        Task<string> GetStringAsync(string key);
        bool SetString(string key, string value, TimeSpan expiresIn);
        Task<bool> SetStringAsync(string key, string value, TimeSpan expiresIn);
        #endregion
    }
}
