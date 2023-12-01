using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.Composers;

public class MongoDbFilterComposer<T> : IFilterComposer<FilterDefinition<T>>
{
    public FilterDefinition<T> Compose(ISimpleFilter simpleFilter)
    {
        var definition = simpleFilter.Operator switch
        {
            Operator.Equal => Builders<T>.Filter.Eq(simpleFilter.FieldKey, simpleFilter.FieldValue),
            Operator.NotEqual => Builders<T>.Filter.Ne(simpleFilter.FieldKey, simpleFilter.FieldValue),
            Operator.In => Builders<T>.Filter.In(simpleFilter.FieldKey, simpleFilter.FieldValue.SmartCast<List<object>>()),
            _ => Builders<T>.Filter.Empty
        };

        return definition;
    }

    public FilterDefinition<T> Compose(IFilter filter)
    {
        var filters = new List<FilterDefinition<T>>();

        foreach (var compoundFilter in filter.CompoundFilters)
        {
            filters.Add(Compose(compoundFilter));
        }

        foreach (var simpleFilter in filter.SimpleFilters)
        {
            filters.Add(Compose(simpleFilter));
        }

        if (filters.Count == 0)
        {
            return Builders<T>.Filter.Empty;
        }

        if (filters.Count == 1)
        {
            return filters.First();
        }

        var filterDefinition = filter.Logic switch
        {
            CompoundLogic.Or => Builders<T>.Filter.Or(filters),
            CompoundLogic.And => Builders<T>.Filter.And(filters),
            _ => Builders<T>.Filter.And(filters)
        };

        return filterDefinition;
    }
}