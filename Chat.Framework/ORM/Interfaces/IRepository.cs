namespace Chat.Framework.ORM.Interfaces;

public interface IRepository<TEntity> where TEntity : class, IRepositoryItem
{
    Task<TEntity?> GetByIdAsync(string id);

    Task<bool> SaveAsync(TEntity entity);

    Task<bool> SaveAsync(List<TEntity> entities);

    Task<bool> DeleteByIdAsync(string id);
}