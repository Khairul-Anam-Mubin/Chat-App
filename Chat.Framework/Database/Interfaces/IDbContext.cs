using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.Interfaces;

public interface IDbContext
{
    Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IEntity;
    Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IEntity;
    
    Task<bool> UpdateOneAsync<T>(DatabaseInfo databaseInfo, ICompoundFilter filter, IUpdateDefinition updateDefinition) where T : class, IEntity;
    Task<bool> UpdateManyAsync<T>(DatabaseInfo databaseInfo, ICompoundFilter filter, IUpdateDefinition updateDefinition) where T : class, IEntity;

    Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity;
    Task<bool> DeleteManyAsync<T>(DatabaseInfo databaseInfo, ICompoundFilter filter) where T : class, IEntity;

    Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity;
    Task<T?> GetOneAsync<T>(DatabaseInfo databaseInfo, ICompoundFilter filter) where T : class, IEntity;

    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, ICompoundFilter filter) where T : class, IEntity;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, ICompoundFilter filter, ISort sort, int offset, int limit) where T: class, IEntity;
    
    Task<string?> CreateIndexAsync<T>(DatabaseInfo databaseInfo, List<FieldOrder> fieldOrders);
}