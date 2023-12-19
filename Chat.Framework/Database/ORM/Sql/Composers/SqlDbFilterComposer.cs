using System.Text;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;

namespace Chat.Framework.Database.ORM.Sql.Composers;

public class SqlDbFilterComposer : IFilterComposer<SqlQuery>
{
    private readonly SqlParameterProvider _parameterProvider;

    public SqlDbFilterComposer(SqlParameterProvider parameterProvider)
    {
        _parameterProvider = parameterProvider;
    }

    public SqlQuery Compose(ISimpleFilter simpleFilter)
    {
        var query = new SqlQuery();
        switch (simpleFilter.Operator)
        {
            case Operator.Equal:
                var fieldKeyParameter = _parameterProvider.GetSqlParameterFieldKey(simpleFilter.FieldKey);
                query.Query = $"({simpleFilter.FieldKey} = @{fieldKeyParameter})";
                query.DynamicParameters.Add(fieldKeyParameter, simpleFilter.FieldValue);
                break;
            case Operator.NotEqual:
                fieldKeyParameter = _parameterProvider.GetSqlParameterFieldKey(simpleFilter.FieldKey);
                query.Query = $"({simpleFilter.FieldKey} != @{fieldKeyParameter})";
                query.DynamicParameters.Add(fieldKeyParameter, simpleFilter.FieldValue);
                break;
            case Operator.In:
                query = ComposeInQuery(simpleFilter);
                break;
        }

        return query;
    }

    private SqlQuery ComposeInQuery(ISimpleFilter filter)
    {
        var fieldValues = filter.FieldValue.SmartCastToList<object>();

        if (!fieldValues.Any())
        {
            return new SqlQuery();
        }

        var sqlQuery = new SqlQuery($"({filter.FieldKey} IN (");

        foreach (var value in fieldValues)
        {
            var fieldKey = _parameterProvider.GetSqlParameterFieldKey(filter.FieldKey);
            if (value == fieldValues.Last())
            {
                sqlQuery.Query += $"@{fieldKey}))";
            }
            else
            {
                sqlQuery.Query += $"@{fieldKey}, ";
            }

            sqlQuery.DynamicParameters.Add(fieldKey, value);
        }
        return sqlQuery;
    }

    public SqlQuery Compose(IFilter filter)
    {
        var @operator = filter.Logic?.ToString();

        var simpleQuery = GetProcessedSimpleFilters(filter.SimpleFilters, @operator!);
        var compoundQuery = GetProcessedCompoundFilters(filter.CompoundFilters, @operator!);

        if (filter.SimpleFilters.Any() && filter.CompoundFilters.Any())
        {
            return new SqlQuery(
                $"({simpleQuery.Query} {@operator} {compoundQuery.Query})", 
                simpleQuery.MergeQueryParameters(compoundQuery.DynamicParameters).DynamicParameters);
        }

        if (filter.SimpleFilters.Any())
        {
            if (filter.SimpleFilters.Count == 1)
            {
                return new SqlQuery(simpleQuery.Query, simpleQuery.DynamicParameters);
            }

            return new SqlQuery($"({simpleQuery.Query})", simpleQuery.DynamicParameters);
        }

        if (filter.CompoundFilters.Count == 1)
        {
            return new SqlQuery(compoundQuery.Query, compoundQuery.DynamicParameters);
        }
        return new SqlQuery($"({compoundQuery.Query})", compoundQuery.DynamicParameters);
    }

    private SqlQuery GetProcessedSimpleFilters(List<ISimpleFilter> simpleFilters, string @operator)
    {
        if (simpleFilters.Any() == false)
        {
            return new SqlQuery();
        }
            
        var builder = new StringBuilder();
        var sqlQuery = new SqlQuery();

        for (var i = 0; i + 1 < simpleFilters.Count; i++)
        {
            var query = Compose(simpleFilters[i]);

            builder.Append(query.Query);
            builder.Append($" {@operator} ");

            sqlQuery.MergeQueryParameters(query.DynamicParameters);
        }

        var composedFilter = Compose(simpleFilters.Last());
        builder.Append(composedFilter.Query);

        sqlQuery.MergeQueryParameters(composedFilter.DynamicParameters);
        sqlQuery.Query = builder.ToString();

        return sqlQuery;
    }

    private SqlQuery GetProcessedCompoundFilters(List<IFilter> filters, string @operator)
    {
        if (filters.Any() == false)
        {
            return new SqlQuery();
        }

        var builder = new StringBuilder();
        var sqlQuery = new SqlQuery();

        for (var i = 0; i + 1 < filters.Count; i++)
        {
            var query = Compose(filters[i]);

            builder.Append(query.Query);
            builder.Append($" {@operator} ");

            sqlQuery.MergeQueryParameters(query.DynamicParameters);
        }

        var composedFilter = Compose(filters.Last());
        builder.Append(composedFilter.Query);

        sqlQuery.MergeQueryParameters(composedFilter.DynamicParameters);
        sqlQuery.Query = builder.ToString();

        return sqlQuery;
    }
}