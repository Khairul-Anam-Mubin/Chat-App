using Chat.Domain.Interfaces;
using Chat.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Chat.Framework.Database.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Chat.Persistence.Repositories;

[ServiceRegister(typeof(IChatRepository), ServiceLifetime.Singleton)]
public class ChatRepository : RepositoryBase<ChatModel>, IChatRepository
{
    public ChatRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    : base(configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>(), mongoDbContext)
    {}

    public async Task<List<ChatModel>> GetChatModelsAsync(string userId, string sendTo, int offset, int limit)
    {
        var userIdFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.UserId, userId);
        var sendToFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.SendTo, sendTo);
        var andFilter = Builders<ChatModel>.Filter.And(userIdFilter, sendToFilter);
        
        var alterUserIdFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.UserId, sendTo);
        var alterSendToFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.SendTo, userId);
        var alterAndFilter = Builders<ChatModel>.Filter.And(alterUserIdFilter, alterSendToFilter);
        
        var orFilter = Builders<ChatModel>.Filter.Or(andFilter, alterAndFilter);
        
        var sortDef = Builders<ChatModel>.Sort.Descending("SentAt");
        
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, orFilter, sortDef, offset, limit);
    }

    public async Task<List<ChatModel>> GetSenderAndReceiverSpecificChatModelsAsync(string senderId, string receiverId)
    {
        var senderFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.UserId, senderId);
        var receiverFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.SendTo, receiverId);
        var statusFilter = Builders<ChatModel>.Filter.Ne(chatModel => chatModel.Status, "Seen");
        var andFilter = Builders<ChatModel>.Filter.And(senderFilter, receiverFilter);
        
        return await DbContext.GetEntitiesByFilterDefinitionAsync(DatabaseInfo, andFilter);
    }
}