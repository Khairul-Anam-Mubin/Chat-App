using Chat.Activity.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using Microsoft.Extensions.Configuration;

namespace Chat.Activity.Infrastructure.Migrations;

public class LastSeenModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IDbContext _dbContext;

    public LastSeenModelIndexCreator(IConfiguration configuration, IDbContext dbContext)
    {
        _databaseInfo = configuration.TryGetConfig<DatabaseInfo>();
        _dbContext = dbContext;
    }

    public void CreateIndexes()
    {
        _dbContext.CreateIndexAsync<LastSeenModel>(_databaseInfo,
            new IndexBuilder<LastSeenModel>()
                .Ascending(o => o.UserId)
                .Build())
            .Wait();
    }
}