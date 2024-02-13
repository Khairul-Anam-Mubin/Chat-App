namespace Chat.Framework.Cache.InMemoryCache;

public interface IInMemoryCache
{
    T? GetByKey<T>(string key);
    void RemoveByKey(string key);
    void SetByKey<T>(string key, T value);
}