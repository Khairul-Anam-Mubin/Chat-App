using Chat.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Migrations;

internal class LatestChatModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public LatestChatModelIndexCreator(DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        _indexManager.CreateOne<LatestChatModel>(_databaseInfo,
            new IndexBuilder<LatestChatModel>()
                .Ascending(o => o.UserId)
                .Ascending(o => o.SendTo)
                .Descending(o => o.SentAt)
                .Build());
    }
}