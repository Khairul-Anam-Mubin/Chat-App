using Chat.Framework.CQRS;

namespace Chat.Contact.Application.Commands;

public class AcceptOrRejectContactRequestCommand : ICommand
{
    public string ContactId { get; set; } = string.Empty;
    public bool IsAcceptRequest { get; set; }
}