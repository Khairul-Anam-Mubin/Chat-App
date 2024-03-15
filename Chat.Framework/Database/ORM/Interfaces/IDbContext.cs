namespace Chat.Framework.Database.ORM.Interfaces;

public interface IDbContext
{
    Task<bool> SaveAsync<T>(DatabaseInfo databaseInfo, T item) where T : class, IRepositoryItem;
    Task<bool> SaveManyAsync<T>(DatabaseInfo databaseInfo, List<T> items) where T : class, IRepositoryItem;

    Task<bool> UpdateOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IRepositoryItem;
    Task<bool> UpdateManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, IUpdate update) where T : class, IRepositoryItem;

    Task<bool> DeleteOneByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem;
    Task<bool> DeleteManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem;

    Task<T?> GetByIdAsync<T>(DatabaseInfo databaseInfo, string id) where T : class, IRepositoryItem;
    Task<T?> GetOneAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem;
    Task<List<T>> GetManyAsync<T>(DatabaseInfo databaseInfo, IFilter filter, ISort sort, int offset, int limit) where T : class, IRepositoryItem;

    Task<long> CountAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem;
    Task<long> CountAsync<T>(DatabaseInfo databaseInfo, IFilter filter) where T : class, IRepositoryItem;
}