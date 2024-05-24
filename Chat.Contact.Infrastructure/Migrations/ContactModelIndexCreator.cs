using Chat.Contacts.Domain.Entities;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Contacts.Infrastructure.Migrations;

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
            new IndexBuilder<Contact>()
                .Ascending(o => o.UserId)
                .Ascending(o => o.IsPending)
                .Build(),
            new IndexBuilder<Contact>()
                .Ascending(o => o.ContactUserId)
                .Ascending(o => o.IsPending)
                .Build()
        };

        _indexManager.CreateMany<Contact>(_databaseInfo, indexes);
    }
}