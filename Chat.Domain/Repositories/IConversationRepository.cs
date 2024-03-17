using Chat.Domain.Entities;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Domain.Repositories;

public interface IConversationRepository : IRepository<Conversation>
{
    Task<Conversation?> GetConversationAsync(string userId, string sendTo);

    Task<List<Conversation>> GetConversationsAsync(string userId, int offset, int limit);
}