using Chat.Framework.ORM.Interfaces;

namespace Chat.Framework.ORM.Sql.Composers;

public class SqlDbUpdateComposer : IUpdateComposer<SqlQuery>
{
    public SqlQuery Compose(IUpdate update)
    {
        if (!update.Fields.Any())
        {
            return new SqlQuery();
        }

        var sqlQuery = new SqlQuery();

        for (var i = 0; i + 1 < update.Fields.Count; i++)
        {
            sqlQuery.Query += $"{update.Fields[i].FieldKey} = @{update.Fields[i].FieldKey}, ";
            sqlQuery.DynamicParameters.TryAdd(update.Fields[i].FieldKey, update.Fields[i].FieldValue!);
        }

        sqlQuery.Query += $"{update.Fields.Last().FieldKey} = @{update.Fields.Last().FieldKey}";
        sqlQuery.DynamicParameters.TryAdd(update.Fields.Last().FieldKey, update.Fields.Last().FieldValue!);

        return sqlQuery;
    }
}