using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Filters;

public class SimpleFilter : IFilter
{
    public string FieldKey { get; set; }
    public Operator Operator { get; set; }
    public object FieldValue { get; set; }

    public SimpleFilter(string fieldKey, Operator @operator, object fieldValue)
    {
        FieldKey = fieldKey;
        Operator = @operator;
        FieldValue = fieldValue;
    }
}
