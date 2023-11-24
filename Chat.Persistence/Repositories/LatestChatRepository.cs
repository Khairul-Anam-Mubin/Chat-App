using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Chat.Framework.Extensions;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Chat.Infrastructure.Repositories;

public class LatestChatRepository : RepositoryBase<LatestChatModel>, ILatestChatRepository
{
    public LatestChatRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    : base(configuration.GetConfig<DatabaseInfo>()!, mongoDbContext)
    {}

    public async Task<LatestChatModel?> GetLatestChatAsync(string userId, string sendTo)
    {
        var userIdFilter = Builders<LatestChatModel>.Filter.Eq("UserId", userId);
        var sendToFilter = Builders<LatestChatModel>.Filter.Eq("SendTo", sendTo);
        var andFilter = Builders<LatestChatModel>.Filter.And(userIdFilter, sendToFilter);
        
        var alterUserIdFilter = Builders<LatestChatModel>.Filter.Eq("UserId", sendTo);
        var alterSendToFilter = Builders<LatestChatModel>.Filter.Eq("SendTo", userId);
        var alterAndFilter = Builders<LatestChatModel>.Filter.And(alterUserIdFilter, alterSendToFilter);
        
        var orFilter = Builders<LatestChatModel>.Filter.Or(andFilter, alterAndFilter);
            
        return await DbContext.GetByFilterDefinitionAsync(DatabaseInfo, orFilter);
    }

    public async Task<List<LatestChatModel>> GetLatestChatModelsAsync(string userId, int offset, int limit)
    {
        var userIdFilter = Builders<LatestChatModel>.Filter.Eq("UserId", userId);
        var sendToFilter = Builders<LatestChatModel>.Filter.Eq("SendTo", userId);
        var orFilter = Builders<LatestChatModel>.Filter.Or(userIdFilter, sendToFilter);
        var sortDef = Builders<LatestChatModel>.Sort.Descending("SentAt");
        
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, orFilter, sortDef , offset, limit);
    }
}