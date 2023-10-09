using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Chat.Identity.Application.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Chat.Identity.Persistence.Repositories;

[ServiceRegister(typeof(IUserRepository), ServiceLifetime.Singleton)]
public class UserRepository : RepositoryBase<UserModel>, IUserRepository
{
    public UserRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    : base(configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>(), mongoDbContext)
    {}

    public async Task<bool> IsUserExistAsync(UserModel userModel)
    {
        var idFilter = Builders<UserModel>.Filter.Eq("Id", userModel.Id);
        var emailFilter = Builders<UserModel>.Filter.Eq("Email", userModel.Email);
        var filter = Builders<UserModel>.Filter.Or(idFilter, emailFilter);
        var userItem = await DbContext.GetByFilterDefinitionAsync(DatabaseInfo, filter);
        return userItem != null;
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        var emailFilter = Builders<UserModel>.Filter.Eq("Email", email);
        var userModel = await DbContext.GetByFilterDefinitionAsync(DatabaseInfo, emailFilter);
        return userModel;
    }

    public async Task<List<UserModel>> GetUsersByUserIdOrEmailAsync(string userId, string email)
    {
        var idFilter = Builders<UserModel>.Filter.Eq("Id", userId);
        var emailFilter = Builders<UserModel>.Filter.Eq("Email", email);
        var filter = Builders<UserModel>.Filter.Or(idFilter, emailFilter);
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, filter);
    }

    private async Task<List<UserModel>> GetUsersByMultipleValuesAsync(string field, List<string> values)
    {
        var filter = Builders<UserModel>.Filter.In(field, values);
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, filter);
    }

    public async Task<List<UserModel>> GetUsersByUserIdsAsync(List<string> userIds)
    {
        return await GetUsersByMultipleValuesAsync("Id", userIds);
    }

    public async Task<List<UserModel>> GetUsersByEmailsAsync(List<string> emails)
    {
        return await GetUsersByMultipleValuesAsync("Email", emails);
    }
}