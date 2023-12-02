using System.Collections.Concurrent;
using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.MongoDb;

public class MongoClientManager : IMongoClientManager
{
    private readonly ConcurrentDictionary<string, MongoClient> _dbClients;

    public MongoClientManager()
    {
        _dbClients = new ConcurrentDictionary<string, MongoClient>();
    }

    public MongoClient GetClient(DatabaseInfo databaseInfo)
    {
        var connectionString = databaseInfo.ConnectionString;

        if (_dbClients.TryGetValue(connectionString, out var client))
        {
            return client;
        }

        var mongoClient = new MongoClient(connectionString);

        _dbClients.TryAdd(connectionString, mongoClient);

        return mongoClient;
    }

    public IMongoDatabase GetDatabase(DatabaseInfo databaseInfo)
    {
        return GetClient(databaseInfo).GetDatabase(databaseInfo.DatabaseName);
    }

    public IMongoCollection<T> GetCollection<T>(DatabaseInfo databaseInfo)
    {
        return GetDatabase(databaseInfo).GetCollection<T>(typeof(T).Name);
    }
}