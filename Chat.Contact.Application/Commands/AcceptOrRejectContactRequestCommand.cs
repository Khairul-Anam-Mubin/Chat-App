namespace Chat.Contact.Application.Commands;

public class AcceptOrRejectContactRequestCommand
{
    public string ContactId { get; set; } = string.Empty;
    public bool IsAcceptRequest { get; set; }
}