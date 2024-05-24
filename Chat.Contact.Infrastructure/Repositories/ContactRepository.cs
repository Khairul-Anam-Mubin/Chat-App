using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Peacious.Framework.ORM;
using Peacious.Framework.ORM.Builders;
using Peacious.Framework.ORM.Enums;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Contacts.Infrastructure.Repositories;

public class ContactRepository : RepositoryBase<Contact>, IContactRepository
{
    public ContactRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo))
    { }

    public async Task<List<Contact>> GetUserContactsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<Contact>();

        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        var contactUserIdFilter = filterBuilder.Eq(o => o.ContactUserId, userId);
        var userFilter = filterBuilder.Or(userIdFilter, contactUserIdFilter);
        var pendingFilter = filterBuilder.Eq(o => o.IsPending, false);
        var filter = filterBuilder.And(userFilter, pendingFilter);
        return await DbContext.GetManyAsync<Contact>(DatabaseInfo, filter);
    }

    public async Task<List<Contact>> GetContactRequestsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<Contact>();

        var userFilter = filterBuilder.Eq(o => o.ContactUserId, userId);
        var pendingFilter = filterBuilder.Eq(o => o.IsPending, true);
        var filter = filterBuilder.And(userFilter, pendingFilter);

        return await DbContext.GetManyAsync<Contact>(DatabaseInfo, filter);
    }

    public async Task<List<Contact>> GetPendingContactsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<Contact>();

        var userFilter = filterBuilder.Eq(o => o.UserId, userId);
        var pendingFilter = filterBuilder.Eq(o => o.IsPending, true);
        var filter = filterBuilder.And(userFilter, pendingFilter);

        return await DbContext.GetManyAsync<Contact>(DatabaseInfo, filter);
    }
}