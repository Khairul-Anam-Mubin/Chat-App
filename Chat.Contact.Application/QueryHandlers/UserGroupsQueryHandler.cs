using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Models;
using Chat.Contact.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Identity;
using Chat.Framework.Results;

namespace Chat.Contact.Application.QueryHandlers;

public class UserGroupsQueryHandler : IQueryHandler<UserGroupsQuery, List<GroupModel>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public UserGroupsQueryHandler(IGroupRepository groupRepository, IScopeIdentity scopeIdentity)
    {
        _groupRepository = groupRepository;
        _scopeIdentity = scopeIdentity;
    }

    public async Task<IResult<List<GroupModel>>> HandleAsync(UserGroupsQuery request)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var groupMemberModels = await _groupRepository.GetUserGroupsAsync(userId);

        var distinctGroupMemberModels = 
            groupMemberModels.DistinctBy(x => x.GroupId);

        var groupIds = 
            distinctGroupMemberModels
            .Select(distinctGroupMemberModel => distinctGroupMemberModel.GroupId)
            .ToList();

        var groups = await _groupRepository.GetGroupsByGroupIds(groupIds);

        return Result.Success(groups);
    }
}
