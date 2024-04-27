using Chat.Domain.Entities;
using KCluster.Framework.ORM;
using KCluster.Framework.ORM.Builders;
using KCluster.Framework.ORM.Interfaces;

namespace Chat.Infrastructure.Migrations;

public class MessageIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public MessageIndexCreator(
        DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        var indexes = new List<IIndex>
        {
            new IndexBuilder<Message>()
                .Ascending(o => o.SenderId)
                .Ascending(o => o.ReceiverId)
                .Descending(o => o.SentAt)
                .Build(),

            new IndexBuilder<Message>()
                .Ascending(o => o.ReceiverId)
                .Ascending(o => o.IsGroupMessage)
                .Descending(o => o.SentAt)
                .Build()
        };

        _indexManager.CreateMany<Message>(_databaseInfo, indexes);
    }
}