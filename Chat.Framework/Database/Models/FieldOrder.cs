using Chat.Framework.Database.ORM.Enums;

namespace Chat.Framework.Database.Models;

public class FieldOrder
{
    public string FieldKey { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; }
}