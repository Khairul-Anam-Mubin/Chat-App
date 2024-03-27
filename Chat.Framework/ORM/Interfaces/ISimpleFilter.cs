using Chat.Framework.ORM.Enums;

namespace Chat.Framework.ORM.Interfaces;

public interface ISimpleFilter
{
    string FieldKey { get; set; }
    Operator Operator { get; set; }
    object FieldValue { get; set; }
}