using Chat.Activity.Domain.Entities;
using Chat.Framework.ORM.Interfaces;

namespace Chat.Activity.Domain.Repositories;

public interface IPresenceRepository : IRepository<Presence>
{
    Task<Presence?> GetPresenceByUserIdAsync(string userId);

    Task<List<Presence>> GetPresenceListByUserIdsAsync(List<string> userIds);

    Task<bool> TrackPresenceAsync(string userId);
}