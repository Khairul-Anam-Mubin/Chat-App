using Chat.Domain.Entities;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Domain.Repositories;

public interface IMessageRepository : IRepository<Message>
{
    Task<List<Message>> GetConversationMessagesAsync(string conversationId, int offset, int limit);

    Task<List<Message>> GetSenderAndReceiverSpecificMessagesAsync(string senderId, string receiverId);

    Task<List<Message>> GetMessagesByIds(List<string> ids);

    Task<List<Message>> GetGroupMessagesAsync(string groupId, int offset, int limit);
}