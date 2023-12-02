using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Infrastructure.Migrations;

public class UserModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public UserModelIndexCreator(DatabaseInfo databaseInfo,IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        _indexManager.CreateOne<UserModel>(_databaseInfo,
            new IndexBuilder<UserModel>()
                .Ascending(o => o.Email)
                .Build()
            );
    }
}