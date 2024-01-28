using Chat.Framework.CQRS;

namespace Chat.Contact.Application.Commands;

public class CreateNewGroupCommand : ICommand
{
    public string GroupName { get; set; }
    public string CreatedBy { get; set; }

    public CreateNewGroupCommand(string groupName, string createdBy)
    {
        GroupName = groupName;
        CreatedBy = createdBy;
    }
}
