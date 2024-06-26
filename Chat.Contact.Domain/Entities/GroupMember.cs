﻿using Peacious.Framework.DDD;
using Peacious.Framework.ORM.Interfaces;
using Peacious.Framework.Results;

namespace Chat.Contacts.Domain.Entities;

public class GroupMember : Entity, IRepositoryItem
{
    public string GroupId { get; private set; }
    public string MemberId { get; private set; }
    public string AddedBy { get; private set; }
    public DateTime JoinedAt { get; private set; }

    private GroupMember(string groupId, string memberId, string addedBy)
        : base(Guid.NewGuid().ToString())
    {
        GroupId = groupId;
        MemberId = memberId;
        AddedBy = addedBy;
        JoinedAt = DateTime.UtcNow;
    }

    public static IResult<GroupMember> Create(string groupId, string memberId, string addedBy)
    {
        return Result.Success(new GroupMember(groupId, memberId, addedBy));
    }
}
