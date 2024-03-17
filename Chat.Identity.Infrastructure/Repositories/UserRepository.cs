using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using Chat.Identity.Domain.Entities;
using Chat.Identity.Domain.Repositories;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(IDbContextFactory dbContextFactory, IConfiguration configuration)
        : base(configuration.TryGetConfig<DatabaseInfo>("DatabaseInfo"), 
            dbContextFactory.GetDbContext(Context.Mongo)) {}

    public async Task<bool> IsUserExistAsync(string email)
    {
        return await GetUserByEmailAsync(email) is not null;
    }

    public async Task<bool> IsUserExistAsync(User userModel)
    {
        var filterBuilder = new FilterBuilder<User>();

        var emailFilter = filterBuilder.Eq(o => o.Email, userModel.Email);
        var idFilter = filterBuilder.Eq(o => o.Id, userModel.Id);
        var filter = filterBuilder.Or(idFilter, emailFilter);
        
        var userItem = await DbContext.GetOneAsync<User>(DatabaseInfo, filter);
        return userItem != null;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var filterBuilder = new FilterBuilder<User>();

        var emailFilter = filterBuilder.Eq(o => o.Email, email);

        var userModel = await DbContext.GetOneAsync<User>(DatabaseInfo, emailFilter);

        return userModel;
    }

    public async Task<List<User>> GetUsersByUserIdOrEmailAsync(string userId, string email)
    {
        var filterBuilder = new FilterBuilder<User>();

        var emailFilter = filterBuilder.Eq(o => o.Email, email);
        var idFilter = filterBuilder.Eq(o => o.Id, userId);
        var filter = filterBuilder.Or(idFilter, emailFilter);

        return await DbContext.GetManyAsync<User>(DatabaseInfo, filter);
    }

    public async Task<List<User>> GetUsersByUserIdsAsync(List<string> userIds)
    {
        var filterBuilder = new FilterBuilder<User>();

        var filter = filterBuilder.In(o => o.Id, userIds);

        return await DbContext.GetManyAsync<User>(DatabaseInfo, filter);
    }

    public async Task<List<User>> GetUsersByEmailsAsync(List<string> emails)
    {
        var filterBuilder = new FilterBuilder<User>();

        var filter = filterBuilder.In(o => o.Email, emails);

        return await DbContext.GetManyAsync<User>(DatabaseInfo, filter);
    }

    public async Task<bool> UpdateEmailVerificationStatus(string userId, bool isVerified)
    {
        var userIdFilter = new FilterBuilder<User>().Eq(o => o.Id, userId);

        var updateFilter = 
            new UpdateBuilder<User>()
            .Set(o => o.IsEmailVerified, isVerified)
            .Build();

        return await DbContext.UpdateOneAsync<User>(DatabaseInfo, userIdFilter, updateFilter);
    }
}