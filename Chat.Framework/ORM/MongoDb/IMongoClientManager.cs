using MongoDB.Driver;

namespace Chat.Framework.ORM.MongoDb;

public interface IMongoClientManager
{
    MongoClient GetClient(DatabaseInfo databaseInfo);

    IMongoDatabase GetDatabase(DatabaseInfo databaseInfo);

    IMongoCollection<T> GetCollection<T>(DatabaseInfo databaseInfo);
}