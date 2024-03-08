using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Entities;
using Chat.Contact.Domain.Repositories;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contact.Application.QueryHandlers;

public class GroupMembersQueryHandler : IQueryHandler<GroupMembersQuery, List<GroupMemberModel>>
{
    private readonly IGroupRepository _groupRepository;
    
    public GroupMembersQueryHandler(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<IResult<List<GroupMemberModel>>> HandleAsync(GroupMembersQuery request)
    {
        var groupMembers = await _groupRepository.GetAllGroupMembers(request.GroupId);

        return Result.Success(groupMembers);
    }
}
