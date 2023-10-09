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

[ServiceRegister(typeof(IAccessRepository), ServiceLifetime.Singleton)]
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