using System.Collections.Concurrent;
using Chat.Framework.ORM;
using StackExchange.Redis;

namespace Chat.Framework.Cache.DistributedCache;

public class RedisClientManager : IRedisClientManager
{
    private readonly ConcurrentDictionary<string, ConnectionMultiplexer> _connections;

    public RedisClientManager()
    {
        _connections = new ConcurrentDictionary<string, ConnectionMultiplexer>();
    }

    public ConnectionMultiplexer GetConnectionMultiplexer(DatabaseInfo databaseInfo)
    {
        if (_connections.TryGetValue(databaseInfo.ConnectionString, out var connectionMultiplexer))
        {
            return connectionMultiplexer;
        }

        connectionMultiplexer = ConnectionMultiplexer.Connect(databaseInfo.ConnectionString);

        _connections.TryAdd(databaseInfo.ConnectionString, connectionMultiplexer);

        return connectionMultiplexer;
    }
}