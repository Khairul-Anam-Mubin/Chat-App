namespace Chat.Framework.Database.ORM.Interfaces;

public interface IMigrationJob
{
    Task MigrateAsync();
}