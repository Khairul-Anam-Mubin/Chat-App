using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Contacts.Application.Commands;

public class CreateNewGroupCommand : ICommand
{
    [Required]
    public string GroupName { get; set; }

    [Required]
    public string UserId { get; set; }

    public CreateNewGroupCommand(string groupName, string userId)
    {
        GroupName = groupName;
        UserId = userId;
    }
}
