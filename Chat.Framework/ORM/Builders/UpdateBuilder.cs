using System.Linq.Expressions;
using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Filters;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Framework.ORM.Builders;

public class UpdateBuilder<TEntity>
{
    private readonly IUpdate _update;

    public UpdateBuilder()
    {
        _update = new Update();
    }

    public UpdateBuilder<TEntity> Set(string fieldKey, object? value)
    {
        _update.Add(new UpdateField(fieldKey, Operation.Set, value));
        return this;
    }

    public UpdateBuilder<TEntity> Set<TField>(Expression<Func<TEntity, TField>> field, TField value)
    {
        return Set(ExpressionHelper.GetFieldKey(field), value);
    }

    public IUpdate Build()
    {
        return _update;
    }
}