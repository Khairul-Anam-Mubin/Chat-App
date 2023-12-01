namespace Chat.Framework.Database.ORM.Interfaces;

public interface ISort
{
    List<ISortField> SortFields { get; set; }
    ISort Add(ISortField field);
}