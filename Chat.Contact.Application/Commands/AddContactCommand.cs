using Peacious.Framework.CQRS;
using System.ComponentModel.DataAnnotations;

namespace Chat.Contacts.Application.Commands;

public class AddContactCommand : ICommand
{
    [Required]
    public string ContactEmail { get; set; } = string.Empty;
}