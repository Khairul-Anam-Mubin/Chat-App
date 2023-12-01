using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Composers;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using MongoDB.Driver;

namespace Chat.Framework.Database.Contexts;

public class MongoDbContext : IDbContext
{
    protected readonly IMongoClientManager MongoClientManager;

    public MongoDbContext(IMongoClientManager mongoClientManager)
    {
        MongoClientManager = mongoClientManager;
    }

    private IMongoCollection<T> GetCollection<T>(DatabaseInfo databaseInfo)
    {
        var client = MongoClientManager.GetClient(databaseInfo);
        var database = client.GetDatabase(databaseInfo.DatabaseName);
        return database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);

            var filter = Builders<T>.Filter.Eq(o => o.Id, item.Id);

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

    public async Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IEntity
    {
        var writeModels = new List<WriteModel<T>>();

        foreach (var item in items)
        {
            var filter = Builders<T>.Filter.Eq(o => o.Id, item.Id);

            var replaceOneModel = new ReplaceOneModel<T>(filter, item)
            {
                IsUpsert = true
            };

            writeModels.Add(replaceOneModel);
        }

        var collection = GetCollection<T>(databaseInfo);

        var writeResult = await collection.BulkWriteAsync(writeModels);

        return writeResult is { IsAcknowledged: true };
    }

    public async Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);

            var filter = Builders<T>.Filter.Eq(o => o.Id, id);

            var deleteResult = await collection.DeleteOneAsync(filter);

            if (deleteResult is not { DeletedCount: > 0 })
            {
                throw new Exception("Delete Problem");
            }
            
            Console.WriteLine($"Successfully Item Deleted, Id: {id}\n");
            
            return true;
        }
        catch (Exception)
        {
            Console.WriteLine($"Problem Delete Item, Id : {id}\n");

            return false;
        }
    }

    public async Task<bool> DeleteManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);

            var composer = new MongoDbComposerFacade<T>();

            var filterDefinition = composer.Compose(filter);

            var deleteResult = await collection.DeleteManyAsync(filterDefinition);

            if (deleteResult is not { DeletedCount: > 0 })
            {
                throw new Exception("Delete Problem");
            }

            Console.WriteLine($"Successfully Delete Items, count : {deleteResult.DeletedCount}\n");

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Delete Items by filter \n{ex.Message}\n");

            return false;
        }
    }

    public async Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity
    {
        var filterBuilder = new FilterBuilder<T>();

        var idFilter = filterBuilder.Eq(entity => entity.Id, id);
        
        return await GetOneAsync<T>(databaseInfo, idFilter);
    }

    public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            
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

    public async Task<T?> GetOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            
            var composer = new MongoDbComposerFacade<T>();

            var filterDefinition = composer.Compose(filter);

            var items = await collection.FindAsync<T>(filterDefinition);
            
            var item = await items.FirstAsync();
            
            Console.WriteLine($"Successfully Get Item by filter : {item.Serialize()}\n");
            
            return item;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Problem Get Item by filter \n{ex.Message}\n");

            return default;
        }
    }

    public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);
            
            var composer = new MongoDbComposerFacade<T>();

            var filterDefinition = composer.Compose(filter);

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


    public async Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, ISort sort, int offset, int limit) where T : class, IEntity
    {
        try
        {
            var collection = GetCollection<T>(databaseInfo);

            var composer = new MongoDbComposerFacade<T>();

            var filterDefinition = composer.Compose(filter);

            var sortDefinition = composer.Compose(sort);

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

    public async Task<bool> UpdateOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IEntity
    {
        var collection = GetCollection<T>(databaseInfo);

        var composer = new MongoDbComposerFacade<T>();

        var filterDefinition = composer.Compose(filter);

        var updateDefinition = composer.Compose(update);
        
        var result = await collection.UpdateOneAsync(filterDefinition, updateDefinition);

        return result.IsModifiedCountAvailable;
    }

    public async Task<bool> UpdateManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IEntity
    {
        var collection = GetCollection<T>(databaseInfo);

        var composer = new MongoDbComposerFacade<T>();

        var filterDefinition = composer.Compose(filter);

        var updateDefinition = composer.Compose(update);
        
        var result = await collection.UpdateManyAsync(filterDefinition, updateDefinition);

        return result.IsModifiedCountAvailable;
    }

    public async Task<string?> CreateIndexAsync<T>(DatabaseInfo databaseInfo, ISort sort) where T : class, IEntity
    {
        var composer = new MongoDbComposerFacade<T>();

        var collection = GetCollection<T>(databaseInfo);

        var createIndexModel = composer.ComposeToCreateIndexModel(sort);

        var indexName = await collection.Indexes.CreateOneAsync(createIndexModel);

        return indexName;
    }
}