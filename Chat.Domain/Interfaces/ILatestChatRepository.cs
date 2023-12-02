using Chat.Domain.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Domain.Interfaces;

public interface ILatestChatRepository : IRepository<LatestChatModel>
{
    Task<LatestChatModel?> GetLatestChatAsync(string userId, string sendTo);

    Task<List<LatestChatModel>> GetLatestChatModelsAsync(string userId, int offset, int limit);
}