using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contact.Application.QueryHandlers;

public class UserGroupsQueryHandler : IQueryHandler<UserGroupsQuery, List<GroupModel>>
{
    private readonly IGroupMemberRepository _groupMemeberRepository;
    private readonly IGroupRepository _groupRepository;

    public UserGroupsQueryHandler(IGroupMemberRepository groupMemeberRepository, IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
        _groupMemeberRepository = groupMemeberRepository;
    }

    public async Task<IResult<List<GroupModel>>> HandleAsync(UserGroupsQuery request)
    {
        var groupMemberModels = await _groupMemeberRepository.GetUserGroupsAsync(request.UserId);

        var distinctGroupMemberModels = groupMemberModels.DistinctBy(x => x.GroupId);

        var groupIds = 
            distinctGroupMemberModels
            .Select(distinctGroupMemberModel => distinctGroupMemberModel.GroupId)
            .ToList();

        var groups = await _groupRepository.GetGroupsByGroupIds(groupIds);

        return Result.Success(groups);
    }
}
