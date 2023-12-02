using Chat.Framework.Database.ORM;
using StackExchange.Redis;

namespace Chat.Framework.Database.Interfaces;

public interface IRedisClientManager
{
    ConnectionMultiplexer GetConnectionMultiplexer(DatabaseInfo  databaseInfo);
}