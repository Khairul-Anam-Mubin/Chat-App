using System.Data;
using Microsoft.Data.SqlClient;

namespace Chat.Framework.Database.ORM.Sql;

public class SqlConnectionManager : ISqlConnectionManager
{
    public IDbConnection CreateConnection(DatabaseInfo databaseInfo)
    {
        return new SqlConnection(databaseInfo.ConnectionString);
    }
}