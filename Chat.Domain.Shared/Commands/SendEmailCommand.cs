using Peacious.Framework.CQRS;
using Peacious.Framework.EmailSenders;
using System.ComponentModel.DataAnnotations;

namespace Chat.Domain.Shared.Commands;

public class SendEmailCommand : ICommand, IInternalMessage
{
    [Required]
    public Email Email { get; set; }
    public string? Token { get; set; }

    public SendEmailCommand()
    {
        Email = new Email();
    }
}