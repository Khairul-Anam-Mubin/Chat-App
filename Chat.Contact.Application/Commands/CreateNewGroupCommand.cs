using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Contacts.Application.Commands;

public class CreateNewGroupCommand : ICommand
{
    [Required]
    public string GroupName { get; set; }

    public CreateNewGroupCommand(string groupName)
    {
        GroupName = groupName;
    }
}
