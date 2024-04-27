using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using KCluster.Framework.DDD;
using KCluster.Framework.EDD;
using KCluster.Framework.ORM;
using KCluster.Framework.ORM.Builders;
using KCluster.Framework.ORM.Enums;
using KCluster.Framework.ORM.Interfaces;

namespace Chat.Infrastructure.Repositories;

public class ConversationRepository : RepositoryBaseWrapper<Conversation>, IConversationRepository
{
    private readonly IMessageRepository _messageRepository;

    public ConversationRepository(
        IDbContextFactory dbContextFactory, 
        DatabaseInfo databaseInfo, 
        IMessageRepository messageRepository,
        IEventService eventService)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo), eventService)
    {
        _messageRepository = messageRepository;
    }

    public async Task<List<Conversation>> GetUserConversationsAsync(string userId, int offset, int limit)
    {
        var filterBuilder = new FilterBuilder<Conversation>();
        var sortBuilder = new SortBuilder<Conversation>();

        var senderIdFilter = filterBuilder.Eq(o => o.SenderId, userId);
        var receiverIdFilter = filterBuilder.Eq(o => o.ReceiverId, userId);

        var orFilter = filterBuilder.Or(senderIdFilter, receiverIdFilter);
        
        var sortDef = sortBuilder.Descending(o => o.SentAt).Build();
        
        return await DbContext.GetManyAsync<Conversation>(DatabaseInfo, orFilter, sortDef , offset, limit);
    }

    public override async Task<bool> SaveAsync(Conversation conversation)
    {
        if (conversation.Messages.Any())
        {
            await _messageRepository.SaveAsync(conversation.Messages);
        }

        return await base.SaveAsync(conversation);
    }
}