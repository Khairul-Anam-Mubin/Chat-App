using Chat.Contacts.Application.Queries;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contacts.Application.QueryHandlers;

public class UserGroupsQueryHandler : IQueryHandler<UserGroupsQuery, List<Group>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public UserGroupsQueryHandler(IGroupRepository groupRepository, IScopeIdentity scopeIdentity)
    {
        _groupRepository = groupRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<List<Group>>> HandleAsync(UserGroupsQuery request)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var groupMembers = await _groupRepository.GetUserGroupsAsync(userId);

        var distinctGroupMembers =
            groupMembers.DistinctBy(x => x.GroupId);

        var groupIds =
            distinctGroupMembers
            .Select(distinctGroupMember => distinctGroupMember.GroupId)
            .ToList();

        var groups = await _groupRepository.GetGroupsByGroupIds(groupIds);

        return Result.Success(groups);
    }
}
