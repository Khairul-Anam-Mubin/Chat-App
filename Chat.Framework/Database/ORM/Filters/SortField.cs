using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Filters;

public class SortField : ISortField
{
    public string FieldKey { get; set; }
    public SortDirection SortDirection { get; set; }

    public SortField(string fieldKey, SortDirection sortDirection)
    {
        FieldKey = fieldKey;
        SortDirection = sortDirection;
    }
}