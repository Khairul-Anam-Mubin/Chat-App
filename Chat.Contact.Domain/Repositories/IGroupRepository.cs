using Chat.Contacts.Domain.Entities;

namespace Chat.Contacts.Domain.Repositories;

public interface IGroupRepository
{
    Task<List<Group>> GetGroupsByGroupIds(List<string> groupIds);

    Task<Group?> GetGroupByIdAsync(string groupId);

    Task<bool> SaveAsync(Group group);

    Task<bool> IsUserAlreadyExistsInGroupAsync(string groupId, string userId);

    Task<List<GroupMember>> GetAllGroupMembers(string groupId);

    Task<List<GroupMember>> GetUserGroupsAsync(string userId);

    Task<bool> SaveGroupMembersAsync(List<GroupMember> groupMembers);
}