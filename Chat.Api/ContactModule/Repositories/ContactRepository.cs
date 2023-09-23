using Chat.Api.ContactModule.Interfaces;
using Chat.Api.ContactModule.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using MongoDB.Driver;

namespace Chat.Api.ContactModule.Repositories
{
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

        public async Task<bool> SaveContactAsync(Contact contact)
        {
            return await _dbContext.SaveItemAsync(_databaseInfo, contact);
        }

        public async Task<List<Contact>> GetUserContactsAsync(string userId)
        {
            var userIdFilter = Builders<Contact>.Filter.Eq("UserId", userId);
            var contactUserIdFilter = Builders<Contact>.Filter.Eq("ContactUserId", userId);
            var userFilter = Builders<Contact>.Filter.Or(userIdFilter, contactUserIdFilter);
            var pendingFilter = Builders<Contact>.Filter.Eq("IsPending", false);
            var filter = Builders<Contact>.Filter.And(userFilter, pendingFilter);
            return await _dbContext.GetItemsByFilterDefinitionAsync<Contact>(_databaseInfo, filter);
        }

        public async Task<List<Contact>> GetContactRequestsAsync(string userId)
        {   
            var userFilter = Builders<Contact>.Filter.Eq("ContactUserId", userId);
            var pendingFilter = Builders<Contact>.Filter.Eq("IsPending", true);
            var filter = Builders<Contact>.Filter.And(userFilter, pendingFilter);
            return await _dbContext.GetItemsByFilterDefinitionAsync<Contact>(_databaseInfo, filter);
        }

        public async Task<List<Contact>> GetPendingContactsAsync(string userId)
        {   
            var userFilter = Builders<Contact>.Filter.Eq("UserId", userId);
            var pendingFilter = Builders<Contact>.Filter.Eq("IsPending", true);
            var filter = Builders<Contact>.Filter.And(userFilter, pendingFilter);
            return await _dbContext.GetItemsByFilterDefinitionAsync<Contact>(_databaseInfo, filter);
        }

        public async Task<Contact?> GetContactByIdAsync(string contactId)
        {
            return await _dbContext.GetItemByIdAsync<Contact>(_databaseInfo, contactId);
        }

        public async Task<bool> DeleteContactById(string contactId)
        {
            return await _dbContext.DeleteItemByIdAsync<Contact>(_databaseInfo, contactId);
        }
    }
}