using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.Composers;

public class MongoDbIndexKeysComposer<T> : ISortComposer<CreateIndexModel<T>>
{
    public CreateIndexModel<T> Compose(ISort sort)
    {
        var indexKeysDictionary = sort.SortFields.ToDictionary(
            key => key.FieldKey,
        value => value.SortDirection.SmartCast<int>());

        var document = new BsonDocument(indexKeysDictionary);

        var createIndexModel = new CreateIndexModel<T>(
            new BsonDocumentIndexKeysDefinition<T>(document));

        return createIndexModel;
    }
}