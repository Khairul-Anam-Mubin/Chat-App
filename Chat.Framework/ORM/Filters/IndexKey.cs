using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Framework.ORM.Filters;

public class IndexKey : IIndexKey
{
    public string FieldKey { get; set; }
    public SortDirection SortDirection { get; set; }

    public IndexKey(string fieldKey, SortDirection sortDirection)
    {
        FieldKey = fieldKey;
        SortDirection = sortDirection;
    }
}