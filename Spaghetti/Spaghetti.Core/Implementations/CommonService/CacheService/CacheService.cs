using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spaghetti.Core.Abstractions.CommonService.CacheService;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spaghetti.Core.Implementations.CommonService.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;
        private const string providerName = "Redis";
        private Lazy<ConnectionMultiplexer> _connection;
        // TODO: ConfigurationHelper
        public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
            //_connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect()); connect to redis
        }

        #region Common
        public bool Remove(string key)
        {
            try
            {
                _logger.LogDebug($"Removing cache item. Key:{key}, server:{providerName}.");
                _cache.Remove(key);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                _logger.LogDebug($"Removing cache item. Key:{key}, server:{providerName}.");
                await _cache.RemoveAsync(key);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public bool KeyExists(string key)
        =>
             GetDatabase().KeyExists(key, CommandFlags.None);
        

        public async Task<bool> KeyExistsAsync(string key)
        =>
             await GetDatabase().KeyExistsAsync(key, CommandFlags.None);
        

        public bool SetExpire(string key, TimeSpan? timespan)
        =>
             GetDatabase().KeyExpire(key, timespan, CommandFlags.None);
        

        public async Task<bool> SetExpireAsync(string key, TimeSpan? timespan)
        =>
             await GetDatabase().KeyExpireAsync(key, timespan, CommandFlags.None);
        

        public bool DeleteKey(string key)
        =>
             GetDatabase().KeyDelete(key, CommandFlags.None);
        

        public async Task<bool> DeleteKeyAsync(string key)
        =>
             await GetDatabase().KeyDeleteAsync(key, CommandFlags.None);
        

        public bool Lock(string key, string value, TimeSpan exprity = default)
        =>
             GetDatabase().LockTake(key, value, exprity, CommandFlags.None);
        

        public async Task<bool> LockAsync(string key, string value, TimeSpan expiry = default)
        =>
             await GetDatabase().LockTakeAsync(key, value, expiry, CommandFlags.None);
        

        public bool Unlock(string key, string value)
        =>
             GetDatabase().LockRelease(key, value, CommandFlags.None);
        

        public async Task<bool> UnlockAsync(string key, string value)
        => 
            await GetDatabase().LockReleaseAsync(key, value, CommandFlags.None);
        #endregion

        #region get and set
        public T Get<T>(string key)
        {
            _logger.LogDebug($"Retrieving cache information for type {typeof(T).FullName}. Key:{key}, server:{providerName}.");
            var value = _cache.GetString(key);
            if (value != null)
                return JsonConvert.DeserializeObject<T>(value);
            return default(T);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            _logger.LogDebug($"Retrieving cache information for type {typeof(T).FullName}. Key:{key}, server:{providerName}.");
            var value = await _cache.GetStringAsync(key);
            if (value != null)
                return JsonConvert.DeserializeObject<T>(value);
            return default(T);

        }

        public bool Set<T>(string key, T value)
        {
            try
            {
                _logger.LogDebug($"Adding or updating cache information for type {typeof(T).FullName}. Key:{key}, server:{providerName}.");
                _cache.SetString(key, JsonConvert.SerializeObject(value));
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public bool Set<T>(string key, T value, TimeSpan expiresIn)
        {
            try
            {
                _logger.LogDebug($"Adding or updating cache information for type {typeof(T).FullName}. Key:{key}, expire time:{expiresIn}, server:{providerName}.");
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiresIn
                };
                _cache.SetString(key, JsonConvert.SerializeObject(value), cacheOptions);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expiresIn)
        {
            try
            {
                _logger.LogDebug($"Adding or updating cache information for type {typeof(T).FullName}. Key:{key}, expire time:{expiresIn}, server:{providerName}.");
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiresIn
                };
                await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), cacheOptions);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public async Task<bool> SetStringAsync(string key, string value, TimeSpan expiresIn)
        {
            try
            {
                _logger.LogDebug($"Adding or updating cache information for type string. Key:{key}, expire time:{expiresIn}, server:{providerName}.");
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiresIn
                };
                await _cache.SetStringAsync(key, value, cacheOptions);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public bool SetString(string key, string value, TimeSpan expiresIn)
        {
            try
            {
                _logger.LogDebug($"Adding or updating cache information for type string. Key:{key}, expire time:{expiresIn}, server:{providerName}.");
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiresIn
                };
                _cache.SetString(key, value, cacheOptions);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return false;
            }
        }

        public string GetString(string key)
        {
            _logger.LogDebug($"Retrieving cache information for type string. Key:{key}, server:{providerName}.");
            return _cache.GetString(key);

        }

        public async Task<String> GetStringAsync(string key)
        {
            _logger.LogDebug($"Retrieving cache information for type string. Key:{key}, server:{providerName}.");
            return await _cache.GetStringAsync(key);
        }
        #endregion

        #region Private method
        private IDatabase GetDatabase()
        =>  
            _connection.Value.GetDatabase();
        
        #endregion
    }
}
