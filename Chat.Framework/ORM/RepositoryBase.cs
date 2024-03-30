using Chat.Framework.ORM.Builders;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Framework.ORM;

public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class, IRepositoryItem
{
    protected readonly IDbContext DbContext;
    protected readonly DatabaseInfo DatabaseInfo;

    protected RepositoryBase(DatabaseInfo databaseInfo, IDbContext dbContext)
    {
        DatabaseInfo = databaseInfo;
        DbContext = dbContext;
    }

    public virtual async Task<TEntity?> GetByIdAsync(string id)
    {
        return await DbContext.GetByIdAsync<TEntity>(DatabaseInfo, id);
    }

    public virtual async Task<bool> SaveAsync(TEntity entity)
    {
        return await DbContext.SaveAsync(DatabaseInfo, entity);
    }

    public virtual async Task<bool> SaveAsync(List<TEntity> entities)
    {
        return await DbContext.SaveManyAsync(DatabaseInfo, entities);
    }

    public virtual async Task<bool> DeleteByIdAsync(string id)
    {
        return await DbContext.DeleteOneByIdAsync<TEntity>(DatabaseInfo, id);
    }

    public virtual async Task<List<TEntity>> GetManyByIdsAsync(List<string> ids)
    {
        var idsFilter = new FilterBuilder<TEntity>().In(entity => entity.Id, ids);

        return await DbContext.GetManyAsync<TEntity>(DatabaseInfo, idsFilter);
    }

    public virtual async Task<List<TEntity>> GetManyAsync()
    {
        return await DbContext.GetManyAsync<TEntity>(DatabaseInfo);
    }
}