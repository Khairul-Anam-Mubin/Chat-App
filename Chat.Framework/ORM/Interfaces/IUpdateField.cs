using Chat.Framework.ORM.Enums;

namespace Chat.Framework.ORM.Interfaces;

public interface IUpdateField
{
    string FieldKey { get; set; }
    Operation Operation { get; set; }
    object? FieldValue { get; set; }
}