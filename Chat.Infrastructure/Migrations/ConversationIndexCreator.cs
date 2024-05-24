using Chat.Domain.Entities;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Interfaces;

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
                .Ascending(o => o.SenderId)
                .Ascending(o => o.ReceiverId)
                .Descending(o => o.SentAt)
                .Build());
    }
}