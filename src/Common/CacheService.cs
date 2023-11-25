using Microsoft.Extensions.Caching.Memory;

namespace Common;

public class CacheService : ICacheService
{
    private readonly IMemoryCache memoryCache;

    public CacheService(IMemoryCache memoryCache) => this.memoryCache = memoryCache;

    public T? Get<T>(string cacheKey) 
    {
        var cacheValue = memoryCache.Get(cacheKey);

        if (cacheValue == null || !(cacheValue is T))
            return default(T);

        return (T)cacheValue;
    }

    public void Add(string cacheKey, TimeSpan duration, object value)
    {
        if (value == null)
            return;

        memoryCache.Set(cacheKey, value, duration);
    }
}
