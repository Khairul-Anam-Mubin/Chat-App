using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contact.Application.QueryHandlers;

public class GroupsQueryHandler : IQueryHandler<GroupsQuery, List<GroupModel>>
{
    private readonly IGroupRepository _groupRepositroy;

    public GroupsQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepositroy = groupRepository;
    }

    public async Task<IResult<List<GroupModel>>> HandleAsync(GroupsQuery request)
    {
        var groups = await _groupRepositroy.GetGroupsByGroupIds(request.GroupIds);

        return Result.Success(groups);
    }
}
