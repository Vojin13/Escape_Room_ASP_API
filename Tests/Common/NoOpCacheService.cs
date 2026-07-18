using Application;

namespace Tests.Common
{
    public class NoOpCacheService : ICacheService
    {
        public T? Get<T>(string key) => default;

        public void Set<T>(string key, T value, TimeSpan? expiration = null)
        {
        }

        public void Remove(string key)
        {
        }
    }
}
