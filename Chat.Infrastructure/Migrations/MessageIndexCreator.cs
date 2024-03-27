using Chat.Framework.ORM;
using Chat.Framework.ORM.Builders;
using Chat.Framework.ORM.Interfaces;

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
            new IndexBuilder<Domain.Entities.Message>()
                .Ascending(o => o.UserId)
                .Ascending(o => o.SendTo)
                .Descending(o => o.SentAt)
                .Build(),

            new IndexBuilder<Domain.Entities.Message>()
                .Ascending(o => o.SendTo)
                .Ascending(o => o.IsGroupMessage)
                .Descending(o => o.SentAt)
                .Build()
        };

        _indexManager.CreateMany<Domain.Entities.Message>(_databaseInfo, indexes);
    }
}