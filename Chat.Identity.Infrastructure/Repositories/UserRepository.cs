using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Database.ORM.Sql.Composers;
using Chat.Framework.Extensions;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class UserRepository : RepositoryBase<UserModel>, IUserRepository
{
    private readonly IDbContext _sqlDbContext;
    private readonly IConfiguration _configuration;

    public UserRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo, IConfiguration configuration)
        : base(configuration.TryGetConfig<DatabaseInfo>("SqlDbConfig"), dbContextFactory.GetDbContext(Context.Sql))
    {
        _configuration = configuration;
        _sqlDbContext = dbContextFactory.GetDbContext(Context.Sql);
    }

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
        var userId = "8cc59c8e-38ec-4805-b692-79d99e43c34e";
        var sqlDbInfo = _configuration.TryGetConfig<DatabaseInfo>("SqlDbConfig");
        var all = await _sqlDbContext.GetManyAsync<UserModel>(sqlDbInfo);
        var user = await _sqlDbContext.GetByIdAsync<UserModel>(sqlDbInfo, userId);
        var count = await _sqlDbContext.CountAsync<UserModel>(sqlDbInfo);

        var filterBuilder = new FilterBuilder<UserModel>();
        var emailFilter = filterBuilder.Eq(o => o.Email, email);
        var idFilter = filterBuilder.Eq(o => o.Id, userId);
        var filter = filterBuilder.Or(idFilter, emailFilter);

        var sqlQuery = new SqlDbFilterComposer().Compose(filter);

        var userModel = await DbContext.GetOneAsync<UserModel>(sqlDbInfo, emailFilter);
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

    public async Task<bool> UpdateEmailVerificationStatus(string userId, bool isVerified)
    {
        var userIdFilter = new FilterBuilder<UserModel>().Eq(o => o.Id, userId);
        var updateFilter = new UpdateBuilder<UserModel>().Set(o => o.IsEmailVerified, isVerified).Build();

        return await DbContext.UpdateOneAsync<UserModel>(DatabaseInfo, userIdFilter, updateFilter);
    }
}