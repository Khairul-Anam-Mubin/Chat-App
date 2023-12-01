using Chat.Framework.Database.ORM.Enums;

namespace Chat.Framework.Database.ORM.Interfaces;

public interface IUpdateField
{
    string FieldKey { get; set; }
    Operation Operation { get; set; }
    object? FieldValue { get; set; }
}