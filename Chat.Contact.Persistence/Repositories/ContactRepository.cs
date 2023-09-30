using Chat.Contact.Application.Interfaces;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Chat.Contact.Persistence.Repositories
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

        public async Task<bool> SaveContactAsync(Domain.Models.Contact contact)
        {
            return await _dbContext.SaveItemAsync(_databaseInfo, contact);
        }

        public async Task<List<Domain.Models.Contact>> GetUserContactsAsync(string userId)
        {
            var userIdFilter = Builders<Domain.Models.Contact>.Filter.Eq("UserId", userId);
            var contactUserIdFilter = Builders<Domain.Models.Contact>.Filter.Eq("ContactUserId", userId);
            var userFilter = Builders<Domain.Models.Contact>.Filter.Or(userIdFilter, contactUserIdFilter);
            var pendingFilter = Builders<Domain.Models.Contact>.Filter.Eq("IsPending", false);
            var filter = Builders<Domain.Models.Contact>.Filter.And(userFilter, pendingFilter);
            return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, filter);
        }

        public async Task<List<Domain.Models.Contact>> GetContactRequestsAsync(string userId)
        {
            var userFilter = Builders<Domain.Models.Contact>.Filter.Eq("ContactUserId", userId);
            var pendingFilter = Builders<Domain.Models.Contact>.Filter.Eq("IsPending", true);
            var filter = Builders<Domain.Models.Contact>.Filter.And(userFilter, pendingFilter);
            return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, filter);
        }

        public async Task<List<Domain.Models.Contact>> GetPendingContactsAsync(string userId)
        {
            var userFilter = Builders<Domain.Models.Contact>.Filter.Eq("UserId", userId);
            var pendingFilter = Builders<Domain.Models.Contact>.Filter.Eq("IsPending", true);
            var filter = Builders<Domain.Models.Contact>.Filter.And(userFilter, pendingFilter);
            return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, filter);
        }

        public async Task<Domain.Models.Contact?> GetContactByIdAsync(string contactId)
        {
            return await _dbContext.GetItemByIdAsync<Domain.Models.Contact>(_databaseInfo, contactId);
        }

        public async Task<bool> DeleteContactById(string contactId)
        {
            return await _dbContext.DeleteItemByIdAsync<Domain.Models.Contact>(_databaseInfo, contactId);
        }
    }
}