using Chat.Contact.Domain.Models;
using Chat.Contact.Domain.Repositories;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Contact.Infrastructure.Repositories;

public class ContactRepository : RepositoryBase<ContactModel>, IContactRepository
{
    public ContactRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo))
    { }

    public async Task<List<ContactModel>> GetUserContactsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<ContactModel>();

        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        var contactUserIdFilter = filterBuilder.Eq(o => o.ContactUserId, userId);
        var userFilter = filterBuilder.Or(userIdFilter, contactUserIdFilter);
        var pendingFilter = filterBuilder.Eq(o => o.IsPending, false);
        var filter = filterBuilder.And(userFilter, pendingFilter);
        return await DbContext.GetManyAsync<ContactModel>(DatabaseInfo, filter);
    }

    public async Task<List<ContactModel>> GetContactRequestsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<ContactModel>();

        var userFilter = filterBuilder.Eq(o => o.ContactUserId, userId);
        var pendingFilter = filterBuilder.Eq(o => o.IsPending, true);
        var filter = filterBuilder.And(userFilter, pendingFilter);
        
        return await DbContext.GetManyAsync<ContactModel>(DatabaseInfo, filter);
    }

    public async Task<List<ContactModel>> GetPendingContactsAsync(string userId)
    {
        var filterBuilder = new FilterBuilder<ContactModel>();

        var userFilter = filterBuilder.Eq(o => o.UserId, userId);
        var pendingFilter = filterBuilder.Eq(o => o.IsPending, true);
        var filter = filterBuilder.And(userFilter, pendingFilter);
        
        return await DbContext.GetManyAsync<ContactModel>(DatabaseInfo, filter);
    }
}