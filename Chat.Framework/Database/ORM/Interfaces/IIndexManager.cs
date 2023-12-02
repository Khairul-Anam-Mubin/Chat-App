namespace Chat.Framework.Database.ORM.Interfaces;

public interface IIndexManager
{
    void CreateOne<T>(DatabaseInfo databaseInfo, IIndex index) where T : class, IEntity;
    Task CreateOneAsync<T>(DatabaseInfo databaseInfo, IIndex index) where T : class, IEntity;

    void CreateMany<T>(DatabaseInfo databaseInfo, List<IIndex> indexes) where T : class, IEntity;
    Task CreateManyAsync<T>(DatabaseInfo databaseInfo, List<IIndex> indexes) where T : class, IEntity;

    void DropAllIndexes<T>(DatabaseInfo databaseInfo) where T : class, IEntity;
    Task DropAllIndexesAsync<T>(DatabaseInfo databaseInfo) where T : class, IEntity;

    void DropIndex<T>(DatabaseInfo databaseInfo, string indexName) where T : class, IEntity;
    Task DropIndexAsync<T>(DatabaseInfo databaseInfo, string indexName) where T : class, IEntity;
}