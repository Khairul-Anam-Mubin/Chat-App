namespace Chat.Framework.Database.ORM.Interfaces;

public interface ISortComposer<out T>
{
    T Compose(ISort sort);
}