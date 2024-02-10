using System.ComponentModel.DataAnnotations;
using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;
using Chat.Framework.MessageBrokers;

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