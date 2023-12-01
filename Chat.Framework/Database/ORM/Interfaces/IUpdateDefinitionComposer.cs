namespace Chat.Framework.Database.ORM.Interfaces;

public interface IUpdateDefinitionComposer<out T>
{
    T Compose(IUpdateDefinition updateDefinition);
}