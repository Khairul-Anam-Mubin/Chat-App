using Chat.Framework.Extensions;
using Chat.Framework.ORM;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Chat.Framework.Cache.DistributedCache;

public class RedisCache : IDistributedCache
{
    private readonly IRedisClientManager _redisClientManager;
    private readonly DatabaseInfo _databaseInfo;

    public RedisCache(IRedisClientManager redisClientManager, IConfiguration configuration) 
    { 
        _redisClientManager = redisClientManager;
        _databaseInfo = configuration.TryGetConfig<DatabaseInfo>("RedisConfig");
    }

    public IDatabase GetDatabase()
    {
        return _redisClientManager.GetConnectionMultiplexer(_databaseInfo).GetDatabase();
    }

    public async Task<bool> ExistByKeyAsync(string key)
    {
        var value = await GetDatabase().StringGetAsync(key);

        return !value.IsNullOrEmpty;
    }

    public async Task<T?> GetByKeyAsync<T>(string key)
    {
        var value = await GetDatabase().StringGetAsync(key);

        return value.SmartCast<T>();
    }

    public async Task RemoveByKeyAsync(string key)
    {
        await GetDatabase().StringGetDeleteAsync(key);
    }

    public async Task SetByKeyAsync<T>(string key, T value)
    {
        await GetDatabase().StringSetAsync(key, value.Serialize());
    }
}
