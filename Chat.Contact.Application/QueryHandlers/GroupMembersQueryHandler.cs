using Chat.Contact.Application.Queries;
using Chat.Contact.Domain.Interfaces;
using Chat.Contact.Domain.Models;
using Chat.Framework.CQRS;
using Chat.Framework.Results;

namespace Chat.Contact.Application.QueryHandlers;

public class GroupMembersQueryHandler : IQueryHandler<GroupMembersQuery, List<GroupMemberModel>>
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    
    public GroupMembersQueryHandler(IGroupMemberRepository groupMemberRepository)
    {
        _groupMemberRepository = groupMemberRepository;
    }

    public async Task<IResult<List<GroupMemberModel>>> HandleAsync(GroupMembersQuery request)
    {
        var groupMembers = await _groupMemberRepository.GetAllGroupMembers(request.GroupId);

        return Result.Success(groupMembers);
    }
}
