using Chat.Framework.ORM.Enums;

namespace Chat.Framework.ORM.Interfaces;

public interface IIndexKey
{
    string FieldKey { get; set; }
    SortDirection SortDirection { get; set; }
}