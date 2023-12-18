using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Framework.Database.ORM.Sql.Composers;

public class SqlDbComposerFacade
{
    private readonly SqlDbFilterComposer _filterComposer;
    private readonly SqlDbUpdateComposer _updateComposer;
    private readonly SqlDbSortComposer _sortComposer;

    public SqlDbComposerFacade()
    {
        _filterComposer = new SqlDbFilterComposer();
        _updateComposer = new SqlDbUpdateComposer();
        _sortComposer = new SqlDbSortComposer();
    }

    public SqlQuery Compose(ISimpleFilter simpleFilter)
    {
        return _filterComposer.Compose(simpleFilter);
    }

    public SqlQuery Compose(IFilter filter)
    {
        return _filterComposer.Compose(filter);
    }

    public string Compose(ISort sort)
    {
        return _sortComposer.Compose(sort);
    }

    public SqlQuery Compose(IUpdate update)
    {
        return _updateComposer.Compose(update);
    }
}