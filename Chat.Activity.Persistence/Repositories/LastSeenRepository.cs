using Chat.Activity.Application.Interfaces.Repositories;
using Chat.Activity.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Chat.Activity.Persistence.Repositories;

[ServiceRegister(typeof(ILastSeenRepository), ServiceLifetime.Singleton)]
public class LastSeenRepository : RepositoryBase<LastSeenModel>, ILastSeenRepository
{
    public LastSeenRepository(IMongoDbContext mongoDbContext, IConfiguration configuration) 
        : base(configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>(), mongoDbContext)
    { }


    public async Task<LastSeenModel?> GetLastSeenModelByUserIdAsync(string userId)
    {
        var userIdFilter = Builders<LastSeenModel>.Filter.Eq(lastSeenModel => lastSeenModel.UserId, userId);
        return await DbContext.GetByFilterDefinitionAsync(DatabaseInfo, userIdFilter);
    }

    public async Task<List<LastSeenModel>> GetLastSeenModelsByUserIdsAsync(List<string> userIds)
    {
        var userIdsFilter = Builders<LastSeenModel>.Filter.In(lastSeenModel => lastSeenModel.UserId, userIds);
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, userIdsFilter);
    }
}