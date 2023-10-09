using Chat.Framework.Database.Models;
using MongoDB.Driver;

namespace Chat.Framework.Database.Interfaces;

public interface IMongoDbContext
{
    Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IEntity;

    Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IEntity;
    
    Task<bool> UpdateOneByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition, UpdateDefinition<T> updateDefinition) where T : class, IEntity;
    
    Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity;
    
    Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity;
    
    Task<List<T>> GetAllAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity;
    
    Task<T?> GetByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IEntity;
    
    Task<bool> DeleteManyByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IEntity;
    
    Task<List<T>> GetEntitiesByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition) where T : class, IEntity;
    
    Task<List<T>> GetEntitiesByFilterDefinitionAsync<T>(DatabaseInfo databaseInfo, FilterDefinition<T> filterDefinition, SortDefinition<T> sortDefinition,int offset, int limit) where T : class, IEntity;
    
    Task<string?> CreateIndexAsync<T>(DatabaseInfo databaseInfo, List<FieldOrder> fieldOrders);
}