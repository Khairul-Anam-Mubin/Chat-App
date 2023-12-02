using Chat.Domain.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Domain.Interfaces;

public interface IChatRepository : IRepository<ChatModel>
{
    Task<List<ChatModel>> GetChatModelsAsync(string userId, string sendTo, int offset, int limit);

    Task<List<ChatModel>> GetSenderAndReceiverSpecificChatModelsAsync(string senderId, string receiverId);
}