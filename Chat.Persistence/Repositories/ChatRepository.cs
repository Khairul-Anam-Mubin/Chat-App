using Chat.Application.Interfaces;
using Chat.Domain.Models;
using Chat.Framework.Attributes;
using Chat.Framework.Database.Interfaces;
using Chat.Framework.Database.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Chat.Persistence.Repositories;

[ServiceRegister(typeof(IChatRepository), ServiceLifetime.Singleton)]
public class ChatRepository : IChatRepository
{
    private readonly IMongoDbContext _dbContext;
    private readonly DatabaseInfo _databaseInfo;

    public ChatRepository(IMongoDbContext mongoDbContext, IConfiguration configuration)
    {
        _dbContext = mongoDbContext;
        _databaseInfo = configuration.GetSection("DatabaseInfo").Get<DatabaseInfo>();
    }

    public async Task<bool> SaveChatModelAsync(ChatModel chatModel)
    {
        return await _dbContext.SaveItemAsync(_databaseInfo, chatModel);
    }

    public async Task<List<ChatModel>> GetChatModelsAsync(string userId, string sendTo, int offset, int limit)
    {
        var userIdFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.UserId, userId);
        var sendToFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.SendTo, sendTo);
        var andFilter = Builders<ChatModel>.Filter.And(userIdFilter, sendToFilter);
        var alterUserIdFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.UserId, sendTo);
        var alterSendToFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.SendTo, userId);
        var alterAndFilter = Builders<ChatModel>.Filter.And(alterUserIdFilter, alterSendToFilter);
        var orFilter = Builders<ChatModel>.Filter.Or(andFilter, alterAndFilter);
        return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, orFilter, offset, limit);
    }

    public async Task<List<ChatModel>> GetSenderAndReceiverSpecificChatModelsAsync(string senderId, string receiverId)
    {
        var senderFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.UserId, senderId);
        var receiverFilter = Builders<ChatModel>.Filter.Eq(chatModel => chatModel.SendTo, receiverId);
        var statusFilter = Builders<ChatModel>.Filter.Ne(chatModel => chatModel.Status, "Seen");
        var andFilter = Builders<ChatModel>.Filter.And(senderFilter, receiverFilter);
        return await _dbContext.GetItemsByFilterDefinitionAsync(_databaseInfo, andFilter);
    }

    public async Task<bool> SaveChatModelsAsync(List<ChatModel> chatModels)
    {
        return await _dbContext.SaveItemsAsync(_databaseInfo, chatModels);
    }
}