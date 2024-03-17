using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;

namespace Chat.Contacts.Application.Commands;

public class AddContactCommand : ICommand
{
    [Required]
    public string ContactEmail { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;
}