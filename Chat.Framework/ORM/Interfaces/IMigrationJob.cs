namespace Chat.Framework.ORM.Interfaces;

public interface IMigrationJob
{
    Task MigrateAsync();
}