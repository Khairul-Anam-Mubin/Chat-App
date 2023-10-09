using Chat.Domain.Models;
using Chat.Framework.Database.Interfaces;

namespace Chat.Application.Interfaces;

public interface IChatRepository : IRepository<ChatModel>
{
    Task<List<ChatModel>> GetChatModelsAsync(string userId, string sendTo, int offset, int limit);
    
    Task<List<ChatModel>> GetSenderAndReceiverSpecificChatModelsAsync(string senderId, string receiverId);
}