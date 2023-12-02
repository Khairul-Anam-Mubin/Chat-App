using Chat.Contact.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Infrastructure.Migrations;

public class ContactModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IDbContext _dbContext;

    public ContactModelIndexCreator(DatabaseInfo databaseInfo, IDbContext dbContext)
    {
        _databaseInfo = databaseInfo;
        _dbContext = dbContext;
    }

    public void CreateIndexes()
    {
        _dbContext.CreateIndexAsync<ContactModel>(_databaseInfo,
                new IndexBuilder<ContactModel>()
                    .Ascending(o => o.UserId)
                    .Ascending(o => o.IsPending)
                    .Build())
            .Wait();

        _dbContext.CreateIndexAsync<ContactModel>(_databaseInfo,
                new IndexBuilder<ContactModel>()
                    .Ascending(o => o.ContactUserId)
                    .Ascending(o => o.IsPending)
                    .Build())
            .Wait();
    }
}