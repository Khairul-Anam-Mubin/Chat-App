using Chat.Contacts.Domain.Entities;
using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Contacts.Application.Queries;

public class GroupsQuery : IQuery<List<Group>>
{
    [Required]
    public List<string> GroupIds { get; set; }

    public GroupsQuery(List<string> groupIds)
    {
        GroupIds = groupIds;
    }
}
