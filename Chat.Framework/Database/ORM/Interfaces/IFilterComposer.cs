namespace Chat.Framework.Database.ORM.Interfaces;

public interface IFilterComposer<out T>
{
    T Compose(ISimpleFilter simpleFilter);
    T Compose(IFilter filter);
}