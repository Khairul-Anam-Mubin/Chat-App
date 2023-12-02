using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.MongoDb;

public interface IMongoClientManager
{
    MongoClient GetClient(DatabaseInfo databaseInfo);

    IMongoDatabase GetDatabase(DatabaseInfo databaseInfo);

    IMongoCollection<T> GetCollection<T>(DatabaseInfo databaseInfo);
}