namespace Chat.Framework.ORM.Interfaces;

public interface IIndexManager
{
    void CreateOne<T>(DatabaseInfo databaseInfo, IIndex index) where T : class, IRepositoryItem;
    Task CreateOneAsync<T>(DatabaseInfo databaseInfo, IIndex index) where T : class, IRepositoryItem;

    void CreateMany<T>(DatabaseInfo databaseInfo, List<IIndex> indexes) where T : class, IRepositoryItem;
    Task CreateManyAsync<T>(DatabaseInfo databaseInfo, List<IIndex> indexes) where T : class, IRepositoryItem;

    void DropAllIndexes<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem;
    Task DropAllIndexesAsync<T>(DatabaseInfo databaseInfo) where T : class, IRepositoryItem;

    void DropIndex<T>(DatabaseInfo databaseInfo, string indexName) where T : class, IRepositoryItem;
    Task DropIndexAsync<T>(DatabaseInfo databaseInfo, string indexName) where T : class, IRepositoryItem;
}