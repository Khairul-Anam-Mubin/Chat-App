namespace Chat.Framework.ORM.Interfaces;

public interface IUpdateComposer<out T>
{
    T Compose(IUpdate update);
}