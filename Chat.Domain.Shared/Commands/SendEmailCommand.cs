using Chat.Framework.CQRS;
using Chat.Framework.EmailSenders;

namespace Chat.Domain.Shared.Commands;

public class SendEmailCommand : ICommand
{
    public Email? Email { get; set; }
}