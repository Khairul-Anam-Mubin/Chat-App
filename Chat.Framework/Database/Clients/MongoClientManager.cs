using System.Collections.Concurrent;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Chat.Framework.Database.Clients;

[ServiceRegister(typeof(IMongoClientManager), ServiceLifetime.Singleton)]
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
}