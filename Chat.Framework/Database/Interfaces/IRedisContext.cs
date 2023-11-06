using Chat.Framework.Database.Models;
using StackExchange.Redis;

namespace Chat.Framework.Database.Interfaces;

public interface IRedisContext
{
    ISubscriber GetSubscriber(DatabaseInfo  databaseInfo);
    Task PublishAsync<T>(DatabaseInfo databaseInfo, string channel, T data);
    Task<bool> SetDataAsync<T>(DatabaseInfo databaseInfo, string key, T value);
    Task<T?> GetDataAsync<T>(DatabaseInfo databaseInfo, string key);
    Task DeleteDataAsync(DatabaseInfo databaseInfo, string key);
    Task<bool> IsDataExistsAsync(DatabaseInfo databaseInfo, string key);
}