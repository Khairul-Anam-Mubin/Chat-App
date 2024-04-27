using KCluster.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

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
