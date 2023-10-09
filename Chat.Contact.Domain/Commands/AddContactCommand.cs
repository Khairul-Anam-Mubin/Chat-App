using Chat.Framework.CQRS;

namespace Chat.Contact.Domain.Commands;

public class AddContactCommand : ACommand
{
    public string ContactEmail { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}