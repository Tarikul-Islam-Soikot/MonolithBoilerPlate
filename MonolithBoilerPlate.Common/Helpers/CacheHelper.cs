using MonolithBoilerPlate.Common.Interface;
using Microsoft.Extensions.Options;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MonolithBoilerPlate.Common.Helpers
{
    public class CacheHelper : ICacheHelper
    {
        private readonly IRedisClient _redisClient;
        private readonly CacheConfig _cacheConfig;

        public CacheHelper(IRedisClient redisClient, IOptions<AppSettings> appSettings)
        {
            _redisClient = redisClient;
            _cacheConfig = appSettings.Value.CacheConfig;
        }

        public async Task<T?> GetAsync<T>(string cacheKey)
        {
            return await _redisClient.Db0.GetAsync<T>(cacheKey);
        }

        public async Task AddAsync(string cacheKey, object data, uint? durationInSeconds = null)
        => await _redisClient.Db0.ReplaceAsync(cacheKey, data, DateTime.Now.AddSeconds(durationInSeconds ?? _cacheConfig.BaseControllerCacheDuration));

        public async Task UpdateAsync(string cacheKey, object data, uint? durationInSeconds = null)
        {
            await RemoveAsync(cacheKey);
            await AddAsync(cacheKey, data, durationInSeconds ?? _cacheConfig.BaseControllerCacheDuration);
        }

        public async Task RemoveAsync(string cacheKey)
        => await _redisClient.Db0.RemoveAsync(cacheKey);

        public async Task RemoveByPatternAsync(string cacheKeyStartsWith)
        {
            var keys = await _redisClient.Db0.SearchKeysAsync($"{cacheKeyStartsWith}*");
            await _redisClient.Db0.RemoveAllAsync(keys.ToArray());
        }
    }
}
