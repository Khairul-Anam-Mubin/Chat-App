using KCluster.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Contacts.Application.Commands;

public class AddMemberToGroupCommand : ICommand
{
    [Required]
    public string GroupId { get; set; }

    [Required]
    public string MemberId { get; set; }

    [Required]
    public string AddedBy { get; set; }

    public AddMemberToGroupCommand(string groupId, string memberId, string addedBy)
    {
        GroupId = groupId;
        MemberId = memberId;
        AddedBy = addedBy;
    }
}
