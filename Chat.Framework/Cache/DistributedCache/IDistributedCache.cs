namespace Chat.Framework.Cache.DistributedCache;

public interface IDistributedCache
{
    Task<T?> GetByKeyAsync<T>(string key);

    Task<bool> ExistByKeyAsync(string key);

    Task RemoveByKeyAsync(string key);

    Task SetByKeyAsync<T>(string key, T value);
}