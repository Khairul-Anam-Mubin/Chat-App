using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Identity.Domain.Models;

namespace Chat.Identity.Infrastructure.Migrations;

public class UserModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IDbContext _dbContext;

    public UserModelIndexCreator(DatabaseInfo databaseInfo, IDbContext dbContext)
    {
        _databaseInfo = databaseInfo;
        _dbContext = dbContext;
    }

    public void CreateIndexes()
    {
        _dbContext.CreateIndexAsync<UserModel>(_databaseInfo,
                new IndexBuilder<UserModel>()
                    .Ascending(o => o.Email)
                    .Build())
            .Wait();
    }
}