using KCluster.Framework.CQRS;

namespace Chat.Contacts.Application.Queries;

public class GroupMembersQuery : IQuery
{
    public string GroupId { get; set; }

    public GroupMembersQuery(string groupId)
    {
        GroupId = groupId;
    }
}
