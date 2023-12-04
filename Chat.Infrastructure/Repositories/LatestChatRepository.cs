using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Repositories;

public class LatestChatRepository : RepositoryBase<LatestChatModel>, ILatestChatRepository
{
    public LatestChatRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo))
    { }

    public async Task<LatestChatModel?> GetLatestChatAsync(string userId, string sendTo)
    {
        var filterBuilder = new FilterBuilder<LatestChatModel>();

        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        var sendToFilter = filterBuilder.Eq(o => o.SendTo, sendTo);
        var andFilter = filterBuilder.And(userIdFilter, sendToFilter);
        
        var alterUserIdFilter = filterBuilder.Eq(o => o.UserId, sendTo);
        var alterSendToFilter = filterBuilder.Eq(o => o.SendTo, userId);
        var alterAndFilter = filterBuilder.And(alterUserIdFilter, alterSendToFilter);
        
        var orFilter = filterBuilder.Or(andFilter, alterAndFilter);
            
        return await DbContext.GetOneAsync<LatestChatModel>(DatabaseInfo, orFilter);
    }

    public async Task<List<LatestChatModel>> GetLatestChatModelsAsync(string userId, int offset, int limit)
    {
        var filterBuilder = new FilterBuilder<LatestChatModel>();
        var sortBuilder = new SortBuilder<LatestChatModel>();

        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        var sendToFilter = filterBuilder.Eq(o => o.SendTo, userId);
        var orFilter = filterBuilder.Or(userIdFilter, sendToFilter);
        
        var sortDef = sortBuilder.Descending(o => o.SentAt).Build();
        
        return await DbContext.GetManyAsync<LatestChatModel>(DatabaseInfo, orFilter, sortDef , offset, limit);
    }
}