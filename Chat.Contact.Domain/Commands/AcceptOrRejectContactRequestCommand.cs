using Chat.Framework.CQRS;

namespace Chat.Contact.Domain.Commands;

public class AcceptOrRejectContactRequestCommand : ACommand
{
    public string ContactId { get; set; } = string.Empty;
    public bool IsAcceptRequest { get; set; }
}