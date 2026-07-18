using Application;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Implementation.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public T? Get<T>(string key)
        {
            var value = _cache.GetString(key);

            if (value == null)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(value);
        }

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            _cache.SetString(key, JsonSerializer.Serialize(value), options);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
