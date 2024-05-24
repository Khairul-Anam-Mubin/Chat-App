using Chat.Contacts.Domain.Entities;
using Peacious.Framework.ORM.Interfaces;

namespace Chat.Contacts.Domain.Repositories;

public interface IGroupMemberRepository : IRepository<GroupMember>
{
    Task<List<GroupMember>> GetAllGroupMembersAsync(string groupId);

    Task<List<GroupMember>> GetUserGroupsAsync(string userId);

    Task<bool> IsUserAlreadyExistInGroupAsync(string groupId, string userId);
}
