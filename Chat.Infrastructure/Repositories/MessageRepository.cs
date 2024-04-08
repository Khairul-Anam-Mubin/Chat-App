using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.DDD;
using Chat.Framework.EDD;
using Chat.Framework.ORM;
using Chat.Framework.ORM.Builders;
using Chat.Framework.ORM.Enums;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Infrastructure.Repositories;

public class MessageRepository : RepositoryBaseWrapper<Message>, IMessageRepository
{
    public MessageRepository(
        IDbContextFactory dbContextFactory,
        DatabaseInfo databaseInfo,
        IEventService eventService)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo), eventService)
    { }

    public async Task<List<Message>> GetConversationMessagesAsync(string conversationId, int offset, int limit)
    {
        var filterBuilder = new FilterBuilder<Message>();
        var sortBuilder = new SortBuilder<Message>();

        var conversationIdFilter = filterBuilder.Eq(message => message.ConversationId, conversationId);
        
        var sort = sortBuilder.Descending(o => o.SentAt).Build();
        
        return await DbContext.GetManyAsync<Message>(DatabaseInfo, conversationIdFilter, sort, offset, limit);
    }

    public async Task<List<Message>> GetGroupMessagesAsync(string groupId, int offset, int limit)
    {
        var filterBuilder = new FilterBuilder<Message>();
        var sortBuilder = new SortBuilder<Message>();

        var groupIdFilter = filterBuilder.Eq(o => o.ReceiverId, groupId);
        var isGroupMessageFilter = filterBuilder.Eq(o => o.IsGroupMessage, true);

        var andFilter = filterBuilder.And(groupIdFilter, isGroupMessageFilter);

        var sort = sortBuilder.Descending(o => o.SentAt).Build();

        return await DbContext.GetManyAsync<Message>(DatabaseInfo, andFilter, sort, offset, limit);
    }

    public async Task<List<Message>> GetMessagesByIds(List<string> ids)
    {
        var filterBuilder = new FilterBuilder<Message>();
        var idsFilter = filterBuilder.In(o => o.Id , ids);
        return await DbContext.GetManyAsync<Message>(DatabaseInfo, idsFilter);
    }

    public async Task<List<Message>> GetSenderAndReceiverSpecificMessagesAsync(string senderId, string receiverId)
    {
        var filterBuilder = new FilterBuilder<Message>();

        var senderFilter = filterBuilder.Eq(o => o.SenderId, senderId);
        var receiverFilter = filterBuilder.Eq(o => o.ReceiverId, receiverId);
        var andFilter = filterBuilder.And(senderFilter, receiverFilter);
        
        return await DbContext.GetManyAsync<Message>(DatabaseInfo, andFilter);
    }
}