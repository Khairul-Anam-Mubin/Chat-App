using Chat.Framework.CQRS;

namespace Chat.Contact.Application.Commands;

public class AddContactCommand : ICommand
{
    public string ContactEmail { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}