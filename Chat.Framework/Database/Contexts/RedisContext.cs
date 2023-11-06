using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Chat.Framework.Database.Contexts;

[ServiceRegister(typeof(IRedisContext), ServiceLifetime.Singleton)]
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

    public ISubscriber GetSubscriber(DatabaseInfo databaseInfo)
    {
        return _redisClientManager.GetConnectionMultiplexer(databaseInfo).GetSubscriber();
    }

    public async Task PublishAsync<T>(DatabaseInfo databaseInfo, string channel, T data)
    {
        await GetSubscriber(databaseInfo).PublishAsync(channel, data.Serialize());
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