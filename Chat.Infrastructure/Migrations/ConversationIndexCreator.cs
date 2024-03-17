using Chat.Domain.Entities;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Migrations;

internal class ConversationIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public ConversationIndexCreator(DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        _indexManager.CreateOne<Conversation>(_databaseInfo,
            new IndexBuilder<Conversation>()
                .Ascending(o => o.UserId)
                .Ascending(o => o.SendTo)
                .Descending(o => o.SentAt)
                .Build());
    }
}