using System.Data;

namespace Chat.Framework.Database.ORM.Sql;

public interface ISqlClientManager
{
    IDbConnection CreateConnection(DatabaseInfo databaseInfo);
}