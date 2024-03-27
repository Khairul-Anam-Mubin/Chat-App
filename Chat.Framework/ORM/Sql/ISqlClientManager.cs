using System.Data;

namespace Chat.Framework.ORM.Sql;

public interface ISqlClientManager
{
    IDbConnection CreateConnection(DatabaseInfo databaseInfo);
}