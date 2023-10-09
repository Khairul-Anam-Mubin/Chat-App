namespace Chat.Framework.Database.Interfaces;

public interface IRepository<TEntity> where TEntity : class , IEntity
{
    Task<TEntity?> GetByIdAsync(string id);

    Task<bool> SaveAsync(TEntity entity);
    
    Task<bool> SaveAsync(List<TEntity> entities);
    
    Task<bool> DeleteByIdAsync(string id);
}