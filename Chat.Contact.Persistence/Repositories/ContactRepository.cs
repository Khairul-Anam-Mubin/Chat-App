using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Chat.Contact.Infrastructure.Repositories;

public class ContactRepository : RepositoryBase<ContactModel>, IContactRepository
{
    public ContactRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    : base(configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>(), mongoDbContext)
    {}

    public async Task<List<ContactModel>> GetUserContactsAsync(string userId)
    {
        var userIdFilter = Builders<ContactModel>.Filter.Eq("UserId", userId);
        var contactUserIdFilter = Builders<ContactModel>.Filter.Eq("ContactUserId", userId);
        var userFilter = Builders<ContactModel>.Filter.Or(userIdFilter, contactUserIdFilter);
        var pendingFilter = Builders<ContactModel>.Filter.Eq("IsPending", false);
        var filter = Builders<ContactModel>.Filter.And(userFilter, pendingFilter);
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, filter);
    }

    public async Task<List<ContactModel>> GetContactRequestsAsync(string userId)
    {
        var userFilter = Builders<ContactModel>.Filter.Eq("ContactUserId", userId);
        var pendingFilter = Builders<ContactModel>.Filter.Eq("IsPending", true);
        var filter = Builders<ContactModel>.Filter.And(userFilter, pendingFilter);
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, filter);
    }

    public async Task<List<ContactModel>> GetPendingContactsAsync(string userId)
    {
        var userFilter = Builders<ContactModel>.Filter.Eq("UserId", userId);
        var pendingFilter = Builders<ContactModel>.Filter.Eq("IsPending", true);
        var filter = Builders<ContactModel>.Filter.And(userFilter, pendingFilter);
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, filter);
    }
}