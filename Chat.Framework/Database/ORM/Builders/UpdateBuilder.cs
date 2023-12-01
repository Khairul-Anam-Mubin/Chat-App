using System.Linq.Expressions;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Filters;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Builders;

public class UpdateBuilder<TEntity>
{
    private readonly IUpdateDefinition _updateDefinition;

    public UpdateBuilder()
    {
        _updateDefinition = new UpdateDefinition();
    }

    public UpdateBuilder<TEntity> Set(string fieldKey, object? value)
    {
        _updateDefinition.Add(new UpdateField(fieldKey, Operation.Set, value));
        return this;
    }

    public UpdateBuilder<TEntity> Set<TField>(Expression<Func<TEntity,TField>> field, TField value)
    {
        return Set(ExpressionHelper.GetFieldKey(field), value);
    }

    public IUpdateDefinition Build()
    {
        return _updateDefinition;
    }
}