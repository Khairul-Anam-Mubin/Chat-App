using Chat.Contacts.Application.Queries;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using KCluster.Framework.CQRS;
using KCluster.Framework.Results;

namespace Chat.Contacts.Application.QueryHandlers;

public class GroupMembersQueryHandler : IQueryHandler<GroupMembersQuery, List<GroupMember>>
{
    private readonly IGroupRepository _groupRepository;
    private readonly IGroupMemberRepository _groupMemberRepository;

    public GroupMembersQueryHandler(IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository)
    {
        _groupRepository = groupRepository;
        _groupMemberRepository = groupMemberRepository;
    }

    public async Task<IResult<List<GroupMember>>> HandleAsync(GroupMembersQuery request)
    {
        var groupMembers = 
            await _groupMemberRepository.GetAllGroupMembersAsync(request.GroupId);

        return Result.Success(groupMembers);
    }
}
