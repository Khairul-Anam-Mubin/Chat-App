using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;

namespace Chat.Domain.Shared.Commands;

public class SendEmailCommand : ICommand
{
    [Required]
    public Email? Email { get; set; }
}