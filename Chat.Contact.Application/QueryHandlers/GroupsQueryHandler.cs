using Chat.Contacts.Application.Queries;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Results;

namespace Chat.Contacts.Application.QueryHandlers;

public class GroupsQueryHandler : IQueryHandler<GroupsQuery, List<Group>>
{
    private readonly IGroupRepository _groupRepositroy;

    public GroupsQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepositroy = groupRepository;
    }

    public async Task<IResult<List<Group>>> HandleAsync(GroupsQuery request)
    {
        var groups = await _groupRepositroy.GetGroupsByGroupIds(request.GroupIds);

        return Result.Success(groups);
    }
}
