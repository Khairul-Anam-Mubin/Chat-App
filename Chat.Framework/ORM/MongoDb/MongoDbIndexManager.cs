using Chat.Framework.ORM.Interfaces;
using Chat.Framework.ORM.MongoDb.Composers;
using MongoDB.Driver;

namespace Chat.Framework.ORM.MongoDb;

public class MongoDbIndexManager : IIndexManager
{
    private readonly IMongoClientManager _mongoClientManager;

    public MongoDbIndexManager(
        IMongoClientManager mongoClientManager)
    {
        _mongoClientManager = mongoClientManager;
    }

    public async Task CreateOneAsync<T>(DatabaseInfo databaseInfo, IIndex index) where T : class, IRepositoryItem
    {
        try
        {
            var composer = new MongoDbComposerFacade<T>();

            var collection = _mongoClientManager.GetCollection<T>(databaseInfo);

            var createIndexModel = composer.Compose(index);

            var indexName = await collection.Indexes.CreateOneAsync(createIndexModel);

            Console.WriteLine($"Index Created : {indexName}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task CreateManyAsync<T>(DatabaseInfo databaseInfo, List<IIndex> indexes) where T : class, IRepositoryItem
    {
        try
        {
            var composer = new MongoDbComposerFacade<T>();

            var collection = _mongoClientManager.GetCollection<T>(databaseInfo);

            var createIndexModels = new List<CreateIndexModel<T>>();

            foreach (var index in indexes)
            {
                createIndexModels.Add(composer.Compose(index));
            }

            var indexNames = await collection.Indexes.CreateManyAsync(createIndexModels);

            Console.WriteLine($"Index Created Count: {indexNames.Count()}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task DropAllIndexesAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem
    {
        var collection = _mongoClientManager.GetCollection<T>(databaseInfo);
        await collection.Indexes.DropAllAsync();
    }

    public async Task DropIndexAsync<T>(DatabaseInfo databaseInfo, string indexName) where T : class, IRepositoryItem
    {
        var collection = _mongoClientManager.GetCollection<T>(databaseInfo);
        await collection.Indexes.DropOneAsync(indexName);
    }

    public void CreateOne<T>(DatabaseInfo databaseInfo, IIndex index) where T : class, IRepositoryItem
    {
        CreateOneAsync<T>(databaseInfo, index).Wait();
    }

    public void CreateMany<T>(DatabaseInfo databaseInfo, List<IIndex> indexes) where T : class, IRepositoryItem
    {
        CreateManyAsync<T>(databaseInfo, indexes).Wait();
    }

    public void DropAllIndexes<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem
    {
        DropAllIndexesAsync<T>(databaseInfo).Wait();
    }

    public void DropIndex<T>(DatabaseInfo databaseInfo, string indexName) where T : class, IRepositoryItem
    {
        DropIndexAsync<T>(databaseInfo, indexName).Wait();
    }
}