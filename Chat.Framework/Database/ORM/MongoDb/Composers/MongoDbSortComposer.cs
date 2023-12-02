using Chat.Framework.Database.ORM.Interfaces;
using MongoDB.Driver;
using SortDirection = Chat.Framework.Database.ORM.Enums.SortDirection;

namespace Chat.Framework.Database.ORM.MongoDb.Composers;

public class MongoDbSortComposer<T> : ISortComposer<SortDefinition<T>>
{
    public SortDefinition<T> Compose(ISort sort)
    {
        var sortDefinitions = new List<SortDefinition<T>>();

        foreach (var sortField in sort.SortFields)
        {
            switch (sortField.SortDirection)
            {
                case SortDirection.Ascending:
                    sortDefinitions.Add(Builders<T>.Sort.Ascending(sortField.FieldKey));
                    break;
                case SortDirection.Descending:
                    sortDefinitions.Add(Builders<T>.Sort.Descending(sortField.FieldKey));
                    break;
            }
        }

        return Builders<T>.Sort.Combine(sortDefinitions);
    }
}