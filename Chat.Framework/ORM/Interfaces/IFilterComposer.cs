namespace Chat.Framework.ORM.Interfaces;

public interface IFilterComposer<out T>
{
    T Compose(ISimpleFilter simpleFilter);
    T Compose(IFilter filter);
}