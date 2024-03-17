using Chat.Contacts.Application.Queries;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contacts.Application.QueryHandlers;

public class GroupMembersQueryHandler : IQueryHandler<GroupMembersQuery, List<GroupMember>>
{
    private readonly IGroupRepository _groupRepository;

    public GroupMembersQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IResult<List<GroupMember>>> HandleAsync(GroupMembersQuery request)
    {
        var groupMembers = await _groupRepository.GetAllGroupMembers(request.GroupId);

        return Result.Success(groupMembers);
    }
}
