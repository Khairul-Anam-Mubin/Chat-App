using Chat.Framework.ORM;
using StackExchange.Redis;

namespace Chat.Framework.Cache.DistributedCache;

public interface IRedisClientManager
{
    ConnectionMultiplexer GetConnectionMultiplexer(DatabaseInfo databaseInfo);
}