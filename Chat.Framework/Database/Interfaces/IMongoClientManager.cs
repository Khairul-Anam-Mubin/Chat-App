using Chat.Framework.Database.Models;
using MongoDB.Driver;

namespace Chat.Framework.Database.Interfaces;

public interface IMongoClientManager
{
    MongoClient GetClient(DatabaseInfo databaseInfo);
}