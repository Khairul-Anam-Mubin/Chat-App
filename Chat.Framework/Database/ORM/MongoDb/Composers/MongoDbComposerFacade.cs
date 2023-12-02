using Chat.Framework.Database.ORM.Interfaces;
using MongoDB.Driver;

namespace Chat.Framework.Database.ORM.MongoDb.Composers;

public class MongoDbComposerFacade<T>
{
    private readonly MongoDbFilterComposer<T> _filterComposer;
    private readonly MongoDbSortComposer<T> _sortComposer;
    private readonly MongoDbUpdateComposer<T> _updateComposer;
    private readonly MongoDbIndexComposer<T> _indexComposer;

    public MongoDbComposerFacade()
    {
        _filterComposer = new MongoDbFilterComposer<T>();
        _sortComposer = new MongoDbSortComposer<T>();
        _updateComposer = new MongoDbUpdateComposer<T>();
        _indexComposer = new MongoDbIndexComposer<T>();
    }

    public FilterDefinition<T> Compose(ISimpleFilter simpleFilter)
    {
        return _filterComposer.Compose(simpleFilter);
    }

    public FilterDefinition<T> Compose(IFilter filter)
    {
        return _filterComposer.Compose(filter);
    }

    public SortDefinition<T> Compose(ISort sort)
    {
        return _sortComposer.Compose(sort);
    }

    public UpdateDefinition<T> Compose(IUpdate update)
    {
        return _updateComposer.Compose(update);
    }

    public CreateIndexModel<T> Compose(IIndex index)
    {
        return _indexComposer.Compose(index);
    }
}