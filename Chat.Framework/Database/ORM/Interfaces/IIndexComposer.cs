namespace Chat.Framework.Database.ORM.Interfaces;

public interface IIndexComposer<out T>
{
    T Compose(IIndex index);
}