namespace Chat.Framework.ORM.Interfaces;

public interface IIndex
{
    List<IIndexKey> IndexKeys { get; set; }

    IIndex Add(IIndexKey indexKey);
}