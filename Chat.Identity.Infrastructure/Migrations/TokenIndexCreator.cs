using KCluster.Framework.ORM;
using KCluster.Framework.ORM.Builders;
using KCluster.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Infrastructure.Migrations;

public class TokenIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public TokenIndexCreator(DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        var indexes = new List<IIndex>
        {
            new IndexBuilder<Token>()
                .Ascending(o => o.AppId)
                .Build(),

            new IndexBuilder<Token>()
                .Ascending(o => o.UserId)
                .Build()
        };

        _indexManager.CreateMany<Token>(_databaseInfo, indexes);
    }
}