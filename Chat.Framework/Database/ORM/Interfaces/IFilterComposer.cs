namespace Chat.Framework.Database.ORM.Interfaces;

public interface IFilterComposer<out T>
{
    T Compose(IFilter filter);
    T Compose(ICompoundFilter compoundFilter);
}