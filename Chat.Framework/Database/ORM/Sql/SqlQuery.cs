namespace Chat.Framework.Database.ORM.Sql;

public class SqlQuery
{
    public string Query { get; set; } = string.Empty;
    public Dictionary<string, object> DynamicParameters { get; set; }

    public SqlQuery()
    {
        DynamicParameters = new ();
    }

    public SqlQuery(string query)
    {
        Query = query;
        DynamicParameters = new();
    }
    
    public SqlQuery(string query, Dictionary<string, object> dynamicParameters)
    {
        Query = query;
        DynamicParameters = dynamicParameters;
    }

    public SqlQuery MergeQueryParameters(Dictionary<string, object> dynamicParameters)
    {
        foreach (var dynamicParameter in dynamicParameters)
        {
            DynamicParameters.TryAdd(dynamicParameter.Key, dynamicParameter.Value);
        }

        return this;
    }
}