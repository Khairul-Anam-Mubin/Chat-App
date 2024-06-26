﻿using Chat.Contacts.Application.Queries;
using Chat.Contacts.Domain.Entities;
using Chat.Contacts.Domain.Repositories;
using Peacious.Framework.CQRS;
using Peacious.Framework.Identity;
using Peacious.Framework.Results;

namespace Chat.Contacts.Application.QueryHandlers;

public class UserGroupsQueryHandler : IQueryHandler<UserGroupsQuery, List<Group>>
{
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IScopeIdentity _scopeIdentity;

    public UserGroupsQueryHandler(IGroupRepository groupRepository, IScopeIdentity scopeIdentity, IGroupMemberRepository groupMemberRepository)
    {
        _groupRepository = groupRepository;
        _scopeIdentity = scopeIdentity;
        _groupMemberRepository = groupMemberRepository;
    }

    public Task<IResult<List<Group>>> Handle(UserGroupsQuery request, CancellationToken cancellationToken)
    {
        return HandleAsync(request);
    }

    public async Task<IResult<List<Group>>> HandleAsync(UserGroupsQuery request)
    {
        var userId = _scopeIdentity.GetUserId()!;

        var groupMembers = await _groupMemberRepository.GetUserGroupsAsync(userId);

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
