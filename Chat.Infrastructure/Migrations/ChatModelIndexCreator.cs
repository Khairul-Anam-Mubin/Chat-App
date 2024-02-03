using Chat.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Migrations;

public class ChatModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public ChatModelIndexCreator(
        DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        var indexes = new List<IIndex>
        {
            new IndexBuilder<ChatModel>()
                .Ascending(o => o.UserId)
                .Ascending(o => o.SendTo)
                .Descending(o => o.SentAt)
                .Build(),

            new IndexBuilder<ChatModel>()
                .Ascending(o => o.SendTo)
                .Ascending(o => o.IsGroupMessage)
                .Descending(o => o.SentAt)
                .Build()
        };

        _indexManager.CreateMany<ChatModel>(_databaseInfo, indexes);
    }
}