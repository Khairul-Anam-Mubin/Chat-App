using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Filters;

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