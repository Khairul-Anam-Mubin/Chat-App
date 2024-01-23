using Chat.Framework.Database.ORM;
using StackExchange.Redis;

namespace Chat.Framework.Database.Interfaces;

public interface IRedisContext
{
    IDatabase GetDatabase(DatabaseInfo databaseInfo);
    Task<bool> SetDataAsync<T>(DatabaseInfo databaseInfo, string key, T value);
    Task<T?> GetDataAsync<T>(DatabaseInfo databaseInfo, string key);
    Task DeleteDataAsync(DatabaseInfo databaseInfo, string key);
    Task<bool> IsDataExistsAsync(DatabaseInfo databaseInfo, string key);
}