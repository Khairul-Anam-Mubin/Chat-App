using Chat.Contacts.Domain.Entities;
using Peacious.Framework.CQRS;

namespace Chat.Contacts.Application.Queries;

public class GroupMembersQuery : IQuery<List<GroupMember>>
{
    public string GroupId { get; set; }

    public GroupMembersQuery(string groupId)
    {
        GroupId = groupId;
    }
}
