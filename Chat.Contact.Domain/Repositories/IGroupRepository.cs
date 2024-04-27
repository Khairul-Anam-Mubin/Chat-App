using Chat.Contacts.Domain.Entities;
using KCluster.Framework.ORM.Interfaces;

namespace Chat.Contacts.Domain.Repositories;

public interface IGroupRepository : IRepository<Group>
{
    Task<List<Group>> GetGroupsByGroupIds(List<string> groupIds);

    Task<Group?> GetGroupByIdAsync(string groupId);

    Task<bool> IsUserAlreadyExistInGroupAsync(string groupId, string userId);
}