using Chat.Domain.Entities;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Domain.Repositories;

public interface IConversationRepository : IRepository<Conversation>
{
    Task<List<Conversation>> GetUserConversationsAsync(string userId, int offset, int limit);
}