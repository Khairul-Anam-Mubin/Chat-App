namespace Chat.Framework.Database.ORM.Sql.Composers;

public class SqlParameterProvider
{
    private readonly Dictionary<string, int> _fieldKeyCounter;

    public SqlParameterProvider()
    {
        _fieldKeyCounter = new();
    }

    public string GetSqlParameterFieldKey(string fieldKey)
    {
        _fieldKeyCounter.TryGetValue(fieldKey, out int value);
        _fieldKeyCounter[fieldKey] = value + 1;
        var sqlParamFieldKey = $"{fieldKey}{value + 1}";
        return sqlParamFieldKey;
    }
}