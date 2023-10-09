using Chat.Domain.Models;
using Chat.Framework.Database.Interfaces;

namespace Chat.Application.Interfaces;

public interface ILatestChatRepository : IRepository<LatestChatModel>
{
    Task<LatestChatModel?> GetLatestChatAsync(string userId, string sendTo);

    Task<List<LatestChatModel>> GetLatestChatModelsAsync(string userId, int offset, int limit);
}