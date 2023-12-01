using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.Composers;

public class MongoDbUpdateDefinitionComposer<T> : IUpdateDefinitionComposer<UpdateDefinition<T>>
{
    public UpdateDefinition<T> Compose(IUpdateDefinition updateDefinition)
    {
        var updateDefinitions = new List<UpdateDefinition<T>>();
        foreach (var field in updateDefinition.Fields)
        {
            switch (field.Operation)
            {
                case Operation.Set:
                    updateDefinitions.Add(Builders<T>.Update.Set(field.FieldKey, field.FieldValue));
                    break;
            }
        }

        return Builders<T>.Update.Combine(updateDefinitions);
    }
}