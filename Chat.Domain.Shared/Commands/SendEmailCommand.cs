using Chat.Framework.EmailSenders;

namespace Chat.Domain.Shared.Commands;

public class SendEmailCommand
{
    public Email? Email { get; set; }
}