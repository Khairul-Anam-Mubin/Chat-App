using Microsoft.Extensions.Caching.Memory;

namespace Chat.Framework.Cache.InMemoryCache;

public class InMemoryCache : IInMemoryCache
{
    private readonly IMemoryCache _cache;

    public InMemoryCache(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? GetByKey<T>(string key)
    {
        return _cache.Get<T>(key);
    }

    public void RemoveByKey(string key)
    {
        _cache.Remove(key);
    }

    public void SetByKey<T>(string key, T value)
    {
        _cache.Set(key, value);
    }
}
