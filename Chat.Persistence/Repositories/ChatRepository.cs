using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.Repositories;
using Chat.Framework.Extensions;
using Microsoft.Extensions.Configuration;

namespace Chat.Infrastructure.Repositories;

public class ChatRepository : RepositoryBase<ChatModel>, IChatRepository
{
    public ChatRepository(IDbContext dbContext, IConfiguration configuration)
    : base(configuration.GetConfig<DatabaseInfo>()!, dbContext)
    {}

    public async Task<List<ChatModel>> GetChatModelsAsync(string userId, string sendTo, int offset, int limit)
    {
        var filterBuilder = new FilterBuilder<ChatModel>();
        var sortBuilder = new SortBuilder<ChatModel>();

        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        var sendToFilter = filterBuilder.Eq(o => o.SendTo, sendTo);
        var andFilter = filterBuilder.And(userIdFilter, sendToFilter);
        
        var alterUserIdFilter = filterBuilder.Eq(o => o.UserId, sendTo);
        var alterSendToFilter = filterBuilder.Eq(o => o.SendTo, userId);
        var alterAndFilter = filterBuilder.And(alterUserIdFilter, alterSendToFilter);
        
        var orFilter = filterBuilder.Or(andFilter, alterAndFilter);
        
        var sort = sortBuilder.Descending(o => o.SentAt).Build();
        
        return await DbContext.GetManyAsync<ChatModel>(DatabaseInfo, orFilter, sort, offset, limit);
    }

    public async Task<List<ChatModel>> GetSenderAndReceiverSpecificChatModelsAsync(string senderId, string receiverId)
    {
        var filterBuilder = new FilterBuilder<ChatModel>();

        var senderFilter = filterBuilder.Eq(o => o.UserId, senderId);
        var receiverFilter = filterBuilder.Eq(o => o.SendTo, receiverId);
        var andFilter = filterBuilder.And(senderFilter, receiverFilter);
        
        return await DbContext.GetManyAsync<ChatModel>(DatabaseInfo, andFilter);
    }
}