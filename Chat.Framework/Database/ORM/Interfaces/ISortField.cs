using Chat.Framework.Database.ORM.Enums;

namespace Chat.Framework.Database.ORM.Interfaces;

public interface ISortField
{
    string FieldKey { get; set; }
    SortDirection SortDirection { get; set; }
}