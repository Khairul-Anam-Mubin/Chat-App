namespace Chat.Framework.Database.ORM.Interfaces;

public interface IUpdate
{
    List<IUpdateField> Fields { get; set; }

    IUpdate Add(IUpdateField field);
}