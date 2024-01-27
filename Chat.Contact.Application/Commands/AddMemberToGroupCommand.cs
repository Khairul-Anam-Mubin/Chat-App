using Chat.Framework.CQRS;

namespace Chat.Contact.Application.Commands;

public class AddMemberToGroupCommand : ICommand
{
    public string GroupId { get; set; }
    public string MemberId { get; set; }
    public string AddedBy { get; set; }

    public AddMemberToGroupCommand(string groupId, string memberId, string addedBy)
    {
        GroupId = groupId;
        MemberId = memberId;
        AddedBy = addedBy;
    }
}
