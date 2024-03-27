using Chat.Framework.ORM;
using Chat.Framework.ORM.Builders;
using Chat.Framework.ORM.Interfaces;
using Chat.Identity.Domain.Entities;

namespace Chat.Identity.Infrastructure.Migrations;

public class UserIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public UserIndexCreator(DatabaseInfo databaseInfo,IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        _indexManager.CreateOne<User>(_databaseInfo,
            new IndexBuilder<User>()
                .Ascending(o => o.Email)
                .Build()
            );
    }
}