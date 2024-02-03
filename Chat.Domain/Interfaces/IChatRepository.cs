using Chat.Domain.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Domain.Interfaces;

public interface IChatRepository : IRepository<ChatModel>
{
    Task<List<ChatModel>> GetChatModelsAsync(string userId, string sendTo, int offset, int limit);

    Task<List<ChatModel>> GetSenderAndReceiverSpecificChatModelsAsync(string senderId, string receiverId);

    Task<List<ChatModel>> GetChatModelsByIds(List<string> ids);

    Task<List<ChatModel>> GetGroupChatModelsAsync(string groupId, int offset, int limit);
}