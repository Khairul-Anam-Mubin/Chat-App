using Chat.Activity.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using Microsoft.Extensions.Configuration;

namespace Chat.Activity.Infrastructure.Migrations;

public class LastSeenModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public LastSeenModelIndexCreator(IConfiguration configuration, IIndexManager indexManager)
    {
        _databaseInfo = configuration.TryGetConfig<DatabaseInfo>();
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        _indexManager.CreateOne<LastSeenModel>(_databaseInfo,
            new IndexBuilder<LastSeenModel>()
                .Ascending(o => o.UserId)
                .Build());
    }
}