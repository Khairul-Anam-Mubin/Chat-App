using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Interfaces;
using Chat.Framework.Extensions;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.Configuration;

namespace Chat.Identity.Infrastructure.Repositories;

public class AccessRepository : RepositoryBase<AccessModel>, IAccessRepository
{
    public AccessRepository(IDbContext dbContext, IConfiguration configuration):
        base(configuration.GetConfig<DatabaseInfo>()!, dbContext)
    { }

    public async Task<bool> DeleteAllTokenByAppId(string appId)
    {
        var filterBuilder = new FilterBuilder<AccessModel>();
        var filter = filterBuilder.Eq(o => o.AppId, appId);
        return await DbContext.DeleteManyAsync<AccessModel>(DatabaseInfo, filter);
    }

    public async Task<bool> DeleteAllTokensByUserId(string userId)
    {
        var filterBuilder = new FilterBuilder<AccessModel>();
        var filter = filterBuilder.Eq(o => o.UserId, userId);
        return await DbContext.DeleteManyAsync<AccessModel>(DatabaseInfo, filter);
    }
}