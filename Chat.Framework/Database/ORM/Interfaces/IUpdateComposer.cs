namespace Chat.Framework.Database.ORM.Interfaces;

public interface IUpdateComposer<out T>
{
    T Compose(IUpdate update);
}