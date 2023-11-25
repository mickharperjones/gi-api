namespace Common;

public interface ICacheService
{
    T? Get<T>(string cacheKey);
    void Add(string cacheKey, TimeSpan duration, object value);
}