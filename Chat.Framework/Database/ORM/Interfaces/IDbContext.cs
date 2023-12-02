namespace Chat.Framework.Database.ORM.Interfaces;

public interface IDbContext
{
    Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IEntity;
    Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IEntity;

    Task<bool> UpdateOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IEntity;
    Task<bool> UpdateManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IEntity;

    Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity;
    Task<bool> DeleteManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity;

    Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IEntity;
    Task<T?> GetOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IEntity;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, ISort sort, int offset, int limit) where T : class, IEntity;
}