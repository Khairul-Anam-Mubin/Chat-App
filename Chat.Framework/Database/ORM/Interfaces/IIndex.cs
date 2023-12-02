namespace Chat.Framework.Database.ORM.Interfaces;

public interface IIndex
{
    List<IIndexKey> IndexKeys { get; set; }

    IIndex Add(IIndexKey indexKey);
}