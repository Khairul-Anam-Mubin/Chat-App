using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Infrastructure.Migrations;

public class AccessModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public AccessModelIndexCreator(DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        var indexes = new List<IIndex>
        {
            new IndexBuilder<AccessModel>()
                .Ascending(o => o.AppId)
                .Build(),

            new IndexBuilder<AccessModel>()
                .Ascending(o => o.UserId)
                .Build()
        };

        _indexManager.CreateMany<AccessModel>(_databaseInfo, indexes);
    }
}