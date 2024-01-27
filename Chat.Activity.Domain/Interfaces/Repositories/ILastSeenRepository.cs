using Chat.Activity.Domain.Models;
using Chat.Framework.Database.ORM.Interfaces;

namespace Chat.Activity.Domain.Interfaces.Repositories;

public interface ILastSeenRepository : IRepository<LastSeenModel>
{
    Task<LastSeenModel?> GetLastSeenModelByUserIdAsync(string userId);

    Task<List<LastSeenModel>> GetLastSeenModelsByUserIdsAsync(List<string> userIds);

    Task<bool> TrackLastSeenAsync(string userId, bool isActive);
}