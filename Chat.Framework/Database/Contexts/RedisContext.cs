using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.ORM;
using Chat.Framework.Extensions;
using StackExchange.Redis;

namespace Chat.Framework.Database.Contexts;

public class RedisContext : IRedisContext
{
    private readonly IRedisClientManager _redisClientManager;

    public RedisContext(IRedisClientManager redisClientManager)
    {
        _redisClientManager = redisClientManager;
    }

    public IDatabase GetDatabase(DatabaseInfo databaseInfo)
    {
        return _redisClientManager.GetConnectionMultiplexer(databaseInfo).GetDatabase();
    }

    public async Task<bool> SetDataAsync<T>(DatabaseInfo databaseInfo, string key, T value)
    {
        return await GetDatabase(databaseInfo).StringSetAsync(key, value.Serialize());
    }

    public async Task<T?> GetDataAsync<T>(DatabaseInfo databaseInfo, string key)
    {
        var data = await GetDatabase(databaseInfo).StringGetAsync(key);
        return data.SmartCast<T>();
    }

    public async Task DeleteDataAsync(DatabaseInfo databaseInfo, string key)
    {
        await GetDatabase(databaseInfo).StringGetDeleteAsync(key);
    }

    public async Task<bool> IsDataExistsAsync(DatabaseInfo databaseInfo, string key)
    {
        var data = await GetDataAsync<string>(databaseInfo, key);
        return !string.IsNullOrEmpty(data);
    }
}