namespace Chat.Framework.ORM.UnitOfWorks;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync();

    void AddCommand(Func<Task> func);
}
