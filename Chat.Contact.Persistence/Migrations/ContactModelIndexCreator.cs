using Chat.Contact.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Infrastructure.Migrations;

public class ContactModelIndexCreator : IIndexCreator
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IIndexManager _indexManager;

    public ContactModelIndexCreator(DatabaseInfo databaseInfo, IIndexManager indexManager)
    {
        _databaseInfo = databaseInfo;
        _indexManager = indexManager;
    }

    public void CreateIndexes()
    {
        var indexes = new List<IIndex>
        {
            new IndexBuilder<ContactModel>()
                .Ascending(o => o.UserId)
                .Ascending(o => o.IsPending)
                .Build(),
            new IndexBuilder<ContactModel>()
                .Ascending(o => o.ContactUserId)
                .Ascending(o => o.IsPending)
                .Build()
        };

        _indexManager.CreateMany<ContactModel>(_databaseInfo, indexes);
    }
}