using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Framework.ORM.Filters;

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