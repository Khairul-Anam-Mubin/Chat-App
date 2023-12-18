using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Filters;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using Dapper;
using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.Sql.Composers
{
    public class SqlDbFilterComposer : IFilterComposer<SqlQuery>
    {
        private readonly Dictionary<string, int> _fieldKeyCounter;

        public SqlDbFilterComposer()
        {
            _fieldKeyCounter = new();
        }

        public SqlQuery Compose(ISimpleFilter simpleFilter)
        {
            var fieldKeyParameter = GetSqlParameterFieldKey(simpleFilter.FieldKey);

            var query = simpleFilter.Operator switch
            {
                Operator.Equal => $"({simpleFilter.FieldKey} = @{fieldKeyParameter})",
                Operator.NotEqual => $"({simpleFilter.FieldKey} != @{fieldKeyParameter})",
                Operator.In => "",
                _ => ""
            };

            return new SqlQuery(query, new Dictionary<string, object>{{fieldKeyParameter, simpleFilter.FieldValue}});
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

        private string GetSqlParameterFieldKey(string fieldKey)
        {
            _fieldKeyCounter.TryGetValue(fieldKey, out int value);
            _fieldKeyCounter[fieldKey] = value + 1;
            var sqlParamFieldKey = $"{fieldKey}{value + 1}";
            return sqlParamFieldKey;
        }
    }
}

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