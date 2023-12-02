using Chat.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Migrations;

public class ChatModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IDbContext _dbContext;

    public ChatModelIndexCreator(
        DatabaseInfo databaseInfo, 
        IDbContext dbContext)
    {
        _databaseInfo = databaseInfo;
        _dbContext = dbContext;
    }

    public void CreateIndexes()
    {
        // _dbContext.DropAllIndexesAsync<ChatModel>(_databaseInfo).Wait();

        _dbContext.CreateIndexAsync<ChatModel>(_databaseInfo,
            new IndexBuilder<ChatModel>()
                .Ascending(o => o.UserId)
                .Ascending(o => o.SendTo)
                .Descending(o => o.SentAt)
                .Build())
            .Wait();
    }
}