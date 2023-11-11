using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Extensions;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat.Framework.Database.Contexts;

[ServiceRegister(typeof(IMongoDbContext), ServiceLifetime.Singleton)]
public class MongoDbContext : IMongoDbContext
{
    protected readonly IMongoClientManager MongoClientManager;

    public MongoDbContext(IMongoClientManager mongoClientManager)
    {
        MongoClientManager = mongoClientManager;
    }

    private IMongoCollection<T>? GetCollection<T>(DatabaseInfo databaseInfo)
    {
        try
        {
            var client = MongoClientManager.GetClient(databaseInfo);
            var database = client?.GetDatabase(databaseInfo.DatabaseName);
            return database?.GetCollection<T>(typeof(T).Name);
        }
        catch (Exception)
        {
            Console.WriteLine($"Get Collection : {typeof(T).Name} from Database : {databaseInfo.DatabaseName} ErrorMessage");
            return null;
        }
    }

    public async Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            var filter = Builders<T>.Filter.Eq("Id", item.Id);
            await collection.ReplaceOneAsync(filter, item, new ReplaceOptions { IsUpsert = true });
            Console.WriteLine($"Successfully Save Item : {item.Serialize()}\n");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Save Item : {item.Serialize()}\n{ex.Message}\n");
            return false;
        }
    }

    public async Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            var filter = Builders<T>.Filter.Eq("Id", id);
            var res = await collection.DeleteOneAsync(filter);
            if (res != null && res.DeletedCount > 0)
            {
                Console.WriteLine($"Successfully Item Deleted, Id: {id}\n");
                return true;
            }
            throw new Exception();
        }
        catch (Exception)
        {
            Console.WriteLine($"Problem Delete Item, Id : {id}\n");
            return false;
        }
    }

    public async Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            var filter = Builders<T>.Filter.Eq("Id", id);
            var items = await collection.FindAsync<T>(filter);
            var item = await items.FirstOrDefaultAsync();
            Console.WriteLine($"Successfully Get Item : {item.Serialize()}\n");
            return item;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Get Item, id : {id}\n{ex.Message}\n");
            return default;
        }
    }

    public async Task<List<T>> GetAllAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            var filter = Builders<T>.Filter.Empty;
            var itemsCursor = await collection.FindAsync<T>(filter);
            var items = await itemsCursor.ToListAsync();
            Console.WriteLine($"Successfully Get items, Count: {items.Count}\n");
            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Get Items\n{ex.Message}\n");
            return new List<T>();
        }
    }

    public async Task<T?> GetByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            var items = await collection.FindAsync<T>(filterDefinition);
            var item = await items.FirstAsync();
            Console.WriteLine($"Successfully Get Item by filter : {item.Serialize()}\n");
            return item;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Get Item by fiter \n{ex.Message}\n");
            return default;
        }
    }

    public async Task<bool> DeleteManyByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            var res = await collection.DeleteManyAsync(filterDefinition);
            if (res != null && res.DeletedCount > 0)
            {
                Console.WriteLine($"Successfully Delete Items, count : {res.DeletedCount}\n");
                return true;
            }
            throw new Exception();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Delete Items by fiter \n{ex.Message}\n");
            return false;
        }
    }

    public async Task<List<T>> GetEntitiesByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            var itemsCursor = await collection.FindAsync<T>(filterDefinition);
            var items = await itemsCursor.ToListAsync();
            Console.WriteLine($"Successfully Get Items by filter count : {items.Count}\n");
            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Get Items by filter\n{ex.Message}\n");
            return new List<T>();
        }
    }

    public async Task<List<T>> GetEntitiesByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition, SortDefinition<T> sortDefinition, int offset, int limit) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            if (collection == null)
            {
                throw new Exception("Collection null");
            }
            
            var itemsCursor = await collection
                .Find(filterDefinition)
                .Sort(sortDefinition)
                .Skip(offset)
                .Limit(limit)
                .ToCursorAsync();
            var items = await itemsCursor.ToListAsync();
            Console.WriteLine($"Successfully Get Items by filter count : {items.Count}\n");
            return items;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Get Items by filter\n{ex.Message}\n");
            return new List<T>();
        }
    }

    public async Task<bool> UpdateOneByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo,
        FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition) where T : class, IEntity
    {
        var collection = GetCollection<T>(databaseInfo);
        if (collection == null)
        {
            throw new Exception("Collection null");
        }
        var result = await collection.UpdateOneAsync(filterDefinition, updateDefinition);
        return result.IsModifiedCountAvailable;
    }

    public async Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IEntity
    {
        var writeModels = new List<WriteModel<T>>();
        foreach (var item in items)
        {
            var filter = Builders<T>.Filter.Eq("Id", item.Id);
            var replaceOneModel = new ReplaceOneModel<T>(filter, item)
            {
                IsUpsert = true
            };
            writeModels.Add(replaceOneModel);
        }
        var collection = GetCollection<T>(databaseInfo);
        if (collection == null)
        {
            throw new Exception("Collection null");
        }
        var writeResult = await collection.BulkWriteAsync(writeModels);
        return writeResult != null && writeResult.IsAcknowledged;
    }

    public async Task<string?> CreateIndexAsync<T>(DatabaseInfo databaseInfo, List<FieldOrder> fieldOrders)
    {
        var fieldOrdersDictionary = fieldOrders.ToDictionary(
            key => key.FieldKey, 
            value => (int)value.SortDirection);

        var collection = GetCollection<T>(databaseInfo) ?? throw new Exception("Collection null");
            
        var document = new BsonDocument(fieldOrdersDictionary);
        var indexName = await collection.Indexes.CreateOneAsync(
            new CreateIndexModel<T>(
                new BsonDocumentIndexKeysDefinition<T>(document)));
        return indexName;
    }
}