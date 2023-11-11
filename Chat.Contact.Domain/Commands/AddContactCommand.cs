namespace Chat.Contact.Domain.Commands;

public class AddContactCommand
{
    public string ContactEmail { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}