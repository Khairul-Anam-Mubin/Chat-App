using System.Data;

namespace Chat.Framework.Database.ORM.Sql;

public interface ISqlConnectionManager
{
    IDbConnection CreateConnection(DatabaseInfo databaseInfo);
}