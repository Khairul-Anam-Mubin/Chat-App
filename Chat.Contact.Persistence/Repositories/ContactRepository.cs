using Chat.Contact.Application.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Chat.Contact.Persistence.Repositories;

[ServiceRegister(typeof(IContactRepository), ServiceLifetime.Singleton)]
public class ContactRepository : IContactRepository
{
    private readonly DatabaseInfo _databaseInfo;
    private readonly IMongoDbContext _dbContext;

    public ContactRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    {
        _databaseInfo = configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>();
        _dbContext = mongoDbContext;
    }

    public async Task<bool> SaveContactAsync(ContactModel contactModel)
    {
        return await _dbContext.SaveItemAsync(_databaseInfo, contactModel);
    }

    public async Task<List<ContactModel>> GetUserContactsAsync(string userId)
    {
        var userIdFilter = Builders<ContactModel>.Filter.Eq("UserId", userId);
        var contactUserIdFilter = Builders<ContactModel>.Filter.Eq("ContactUserId", userId);
        var userFilter = Builders<ContactModel>.Filter.Or(userIdFilter, contactUserIdFilter);
        var pendingFilter = Builders<ContactModel>.Filter.Eq("IsPending", false);
        var filter = Builders<ContactModel>.Filter.And(userFilter, pendingFilter);
        return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, filter);
    }

    public async Task<List<ContactModel>> GetContactRequestsAsync(string userId)
    {
        var userFilter = Builders<ContactModel>.Filter.Eq("ContactUserId", userId);
        var pendingFilter = Builders<ContactModel>.Filter.Eq("IsPending", true);
        var filter = Builders<ContactModel>.Filter.And(userFilter, pendingFilter);
        return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, filter);
    }

    public async Task<List<ContactModel>> GetPendingContactsAsync(string userId)
    {
        var userFilter = Builders<ContactModel>.Filter.Eq("UserId", userId);
        var pendingFilter = Builders<ContactModel>.Filter.Eq("IsPending", true);
        var filter = Builders<ContactModel>.Filter.And(userFilter, pendingFilter);
        return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, filter);
    }

    public async Task<ContactModel?> GetContactByIdAsync(string contactId)
    {
        return await _dbContext.GetItemByIdAsync<ContactModel>(_databaseInfo, contactId);
    }

    public async Task<bool> DeleteContactById(string contactId)
    {
        return await _dbContext.DeleteItemByIdAsync<ContactModel>(_databaseInfo, contactId);
    }
}