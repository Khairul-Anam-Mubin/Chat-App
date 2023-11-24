using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Chat.Identity.Domain.Interfaces;
using Chat.Identity.Domain.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Chat.Identity.Infrastructure.Repositories;

public class AccessRepository : RepositoryBase<AccessModel>, IAccessRepository
{
    public AccessRepository(IMongoDbContext mongoDbContext, IConfiguration configuration):
        base(configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>(), mongoDbContext)
    { }

    public async Task<bool> DeleteAllTokenByAppId(string appId)
    {
        var filter = Builders<AccessModel>.Filter.Eq(accessModel => accessModel.AppId, appId);
        return await DbContext.DeleteManyByFilterDefinitionAsync(DatabaseInfo, filter);
    }

    public async Task<bool> DeleteAllTokensByUserId(string userId)
    {
        var filter = Builders<AccessModel>.Filter.Eq(accessModel => accessModel.UserId, userId);
        return await DbContext.DeleteManyByFilterDefinitionAsync(DatabaseInfo, filter);
    }
}