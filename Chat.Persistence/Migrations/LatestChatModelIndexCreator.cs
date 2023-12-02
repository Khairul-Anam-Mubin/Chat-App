using Chat.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Migrations;

internal class LatestChatModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IDbContext _dbContext;

    public LatestChatModelIndexCreator(DatabaseInfo databaseInfo, IDbContext dbContext)
    {
        _databaseInfo = databaseInfo;
        _dbContext = dbContext;
    }

    public void CreateIndexes()
    {
        _dbContext.CreateIndexAsync<LatestChatModel>(_databaseInfo,
                new IndexBuilder<LatestChatModel>()
                    .Ascending(o => o.UserId)
                    .Ascending(o => o.SendTo)
                    .Descending(o => o.SentAt)
                    .Build())
            .Wait();
    }
}