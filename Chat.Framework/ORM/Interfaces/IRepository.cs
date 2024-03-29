namespace Chat.Framework.ORM.Interfaces;

public interface IRepository<TEntity> where TEntity : class, IRepositoryItem
{
    Task<TEntity?> GetByIdAsync(string id);

    Task<List<TEntity>> GetManyByIds(List<string> ids);

    Task<bool> SaveAsync(TEntity entity);

    Task<bool> SaveAsync(List<TEntity> entities);

    Task<bool> DeleteByIdAsync(string id);
}