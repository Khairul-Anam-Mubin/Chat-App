using Chat.Contact.Domain.Entities;

namespace Chat.Contact.Domain.Repositories;

public interface IGroupRepository
{
    Task<List<GroupModel>> GetGroupsByGroupIds(List<string> groupIds);

    Task<GroupModel?> GetGroupByIdAsync(string groupId);

    Task<bool> SaveAsync(GroupModel group);

    Task<bool> IsUserAlreadyExistsInGroupAsync(string groupId, string userId);

    Task<List<GroupMemberModel>> GetAllGroupMembers(string groupId);

    Task<List<GroupMemberModel>> GetUserGroupsAsync(string userId);

    Task<bool> SaveGroupMembersAsync(List<GroupMemberModel> groupMembers);
}