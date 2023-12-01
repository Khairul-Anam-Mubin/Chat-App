namespace Chat.Framework.Database.ORM.Interfaces;

public interface IUpdateDefinition
{
    List<IUpdateField> Fields { get; set; }

    IUpdateDefinition Add(IUpdateField field);
}