using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.Repositories;
using Chat.Framework.Extensions;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<UserModel>, IUserRepository
{
    public UserRepository(IDbContext dbContext, IConfiguration configuration)
    : base(configuration.GetConfig<DatabaseInfo>()!, dbContext)
    {}

    public async Task<bool> IsUserExistAsync(UserModel userModel)
    {
        var filterBuilder = new FilterBuilder<UserModel>();

        var emailFilter = filterBuilder.Eq(o => o.Email, userModel.Email);
        var idFilter = filterBuilder.Eq(o => o.Id, userModel.Id);
        var filter = filterBuilder.Or(idFilter, emailFilter);
        
        var userItem = await DbContext.GetOneAsync<UserModel>(DatabaseInfo, filter);
        return userItem != null;
    }

    public async Task<UserModel?> GetUserByEmailAsync(string email)
    {
        var filterBuilder = new FilterBuilder<UserModel>();
        var emailFilter = filterBuilder.Eq(o => o.Email, email);
        var userModel = await DbContext.GetOneAsync<UserModel>(DatabaseInfo, emailFilter);
        return userModel;
    }

    public async Task<List<UserModel>> GetUsersByUserIdOrEmailAsync(string userId, string email)
    {
        var filterBuilder = new FilterBuilder<UserModel>();
        var emailFilter = filterBuilder.Eq(o => o.Email, email);
        var idFilter = filterBuilder.Eq(o => o.Id, userId);
        var filter = filterBuilder.Or(idFilter, emailFilter);
        return await DbContext.GetManyAsync<UserModel>(DatabaseInfo, filter);
    }

    public async Task<List<UserModel>> GetUsersByUserIdsAsync(List<string> userIds)
    {
        var filterBuilder = new FilterBuilder<UserModel>();
        var filter = filterBuilder.In(o => o.Id, userIds);
        return await DbContext.GetManyAsync<UserModel>(DatabaseInfo, filter);
    }

    public async Task<List<UserModel>> GetUsersByEmailsAsync(List<string> emails)
    {
        var filterBuilder = new FilterBuilder<UserModel>();
        var filter = filterBuilder.In(o => o.Email, emails);
        return await DbContext.GetManyAsync<UserModel>(DatabaseInfo, filter);
    }
}