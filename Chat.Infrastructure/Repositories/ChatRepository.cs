using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Repositories;

public class ChatRepository : RepositoryBase<ChatModel>, IChatRepository
{
    public ChatRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo))
    { }

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

    public async Task<List<ChatModel>> GetGroupChatModelsAsync(string groupId, int offset, int limit)
    {
        var filterBuilder = new FilterBuilder<ChatModel>();
        var sortBuilder = new SortBuilder<ChatModel>();

        var groupIdFilter = filterBuilder.Eq(o => o.SendTo, groupId);
        var isGroupMessageFilter = filterBuilder.Eq(o => o.IsGroupMessage, true);
        var andFilter = filterBuilder.And(groupIdFilter, isGroupMessageFilter);

        var sort = sortBuilder.Descending(o => o.SentAt).Build();

        return await DbContext.GetManyAsync<ChatModel>(DatabaseInfo, andFilter, sort, offset, limit);
    }

    public async Task<List<ChatModel>> GetChatModelsByIds(List<string> ids)
    {
        var filterBuilder = new FilterBuilder<ChatModel>();
        var idsFilter = filterBuilder.In(o => o.Id , ids);
        return await DbContext.GetManyAsync<ChatModel>(DatabaseInfo, idsFilter);
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