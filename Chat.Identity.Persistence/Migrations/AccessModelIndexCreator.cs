using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Infrastructure.Migrations;

public class AccessModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IDbContext _dbContext;

    public AccessModelIndexCreator(DatabaseInfo databaseInfo, IDbContext dbContext)
    {
        _databaseInfo = databaseInfo;
        _dbContext = dbContext;
    }

    public void CreateIndexes()
    {
        _dbContext.CreateIndexAsync<AccessModel>(_databaseInfo,
                new IndexBuilder<AccessModel>()
                    .Ascending(o => o.AppId)
                    .Build())
            .Wait();

        _dbContext.CreateIndexAsync<AccessModel>(_databaseInfo,
                new IndexBuilder<AccessModel>()
                    .Ascending(o => o.UserId)
                    .Build())
            .Wait();
    }
}