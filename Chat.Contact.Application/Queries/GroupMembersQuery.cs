using Chat.Framework.CQRS;

namespace Chat.Contact.Application.Queries;

public class GroupMembersQuery : IQuery
{
    public string GroupId { get; set; }

    public GroupMembersQuery(string groupId)
    {
        GroupId = groupId;
    }
}
