using Chat.Framework.ORM.Enums;

namespace Chat.Framework.ORM.Interfaces;

public interface ISortField
{
    string FieldKey { get; set; }
    SortDirection SortDirection { get; set; }
}