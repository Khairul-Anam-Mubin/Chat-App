using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Framework.Database.ORM;
using Chat.Framework.Database.ORM.Builders;
using Chat.Framework.Database.ORM.Enums;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Infrastructure.Repositories;

public class ConversationRepository : RepositoryBase<Conversation>, IConversationRepository
{
    private readonly IMessageRepository _messageRepository;

    public ConversationRepository(IDbContextFactory dbContextFactory, DatabaseInfo databaseInfo, IMessageRepository messageRepository)
        : base(databaseInfo, dbContextFactory.GetDbContext(Context.Mongo))
    {
        _messageRepository = messageRepository;
    }

    public async Task<Conversation?> GetConversationAsync(string userId, string sendTo)
    {
        var filterBuilder = new FilterBuilder<Conversation>();

        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        var sendToFilter = filterBuilder.Eq(o => o.SendTo, sendTo);
        var andFilter = filterBuilder.And(userIdFilter, sendToFilter);
        
        var alterUserIdFilter = filterBuilder.Eq(o => o.UserId, sendTo);
        var alterSendToFilter = filterBuilder.Eq(o => o.SendTo, userId);
        var alterAndFilter = filterBuilder.And(alterUserIdFilter, alterSendToFilter);
        
        var orFilter = filterBuilder.Or(andFilter, alterAndFilter);
            
        return await DbContext.GetOneAsync<Conversation>(DatabaseInfo, orFilter);
    }

    public async Task<List<Conversation>> GetConversationsAsync(string userId, int offset, int limit)
    {
        var filterBuilder = new FilterBuilder<Conversation>();
        var sortBuilder = new SortBuilder<Conversation>();

        var userIdFilter = filterBuilder.Eq(o => o.UserId, userId);
        var sendToFilter = filterBuilder.Eq(o => o.SendTo, userId);
        var orFilter = filterBuilder.Or(userIdFilter, sendToFilter);
        
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