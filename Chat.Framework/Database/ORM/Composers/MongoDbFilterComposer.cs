using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.Composers;

public class MongoDbFilterComposer<T> : IFilterComposer<FilterDefinition<T>>
{
    public FilterDefinition<T> Compose(IFilter filter)
    {
        var definition = filter.Operator switch
        {
            Operator.Equal => Builders<T>.Filter.Eq(filter.FieldKey, filter.FieldValue),
            Operator.NotEqual => Builders<T>.Filter.Ne(filter.FieldKey, filter.FieldValue),
            _ => Builders<T>.Filter.Empty
        };

        return definition;
    }

    public FilterDefinition<T> Compose(ICompoundFilter compoundFilter)
    {
        var filters = new List<FilterDefinition<T>>();

        foreach (var filter in compoundFilter.CompoundFilters)
        {
            filters.Add(Compose(filter));
        }

        foreach (var filter in compoundFilter.Filters)
        {
            filters.Add(Compose(filter));
        }

        if (filters.Count == 0)
        {
            return Builders<T>.Filter.Empty;
        }

        if (filters.Count == 1)
        {
            return filters.First();
        }

        var filterDefinition = compoundFilter.Logic switch
        {
            CompoundLogic.Or => Builders<T>.Filter.Or(filters),
            CompoundLogic.And => Builders<T>.Filter.And(filters),
            _ => Builders<T>.Filter.And(filters)
        };

        return filterDefinition;
    }
}